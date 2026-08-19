using System.Text;
using System.Text.Json.Serialization;
using HospitalSaoJose.Api.Converters;
using HospitalSaoJose.Api.Filters;
using HospitalSaoJose.Api.Security;
using HospitalSaoJose.Api.Token;
using HospitalSaoJose.Application;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Domain.Security.Tokens;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Infrastructure;
using HospitalSaoJose.Infrastructure.DataAccess;
using HospitalSaoJose.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

const string CORS_POLICY = "HospitalSaoJoseSite";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new StringConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Hospital São José", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe apenas o token JWT retornado por POST /auth/login."
    });

    options.AddSecurityRequirement(openApiDocument => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", openApiDocument), [] }
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAccessTokenProvider, HttpContextTokenProvider>();

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 26 * 1024 * 1024);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Sem isto o .NET renomeia as claims registradas do JWT para os tipos
        // WS-Federation — `sub` vira
        // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier.
        // O OnTokenValidated abaixo procura por `sub`, não acharia nada e
        // recusaria toda requisição autenticada com 401.
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SigningKey")!))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                // Um token continua criptograficamente válido depois que o usuário é
                // desativado; por isso a checagem contra o banco a cada requisição.
                var subject = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (Guid.TryParse(subject, out var userId).Equals(false))
                {
                    context.Fail(ErrorMessages.VALIDATION_ACCESS_DENIED);
                    return;
                }

                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyRepository>();

                var user = await userRepository.GetById(userId);
                if (user is null)
                    context.Fail(ErrorMessages.VALIDATION_ACCESS_DENIED);
            },
            OnChallenge = context =>
            {
                context.HandleResponse();

                var expired = context.AuthenticateFailure is SecurityTokenExpiredException;

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = expired
                    ? new ResponseErrorJson(ErrorMessages.VALIDATION_ACCESS_TOKEN_EXPIRED, accessTokenExpired: true)
                    : new ResponseErrorJson(ErrorMessages.VALIDATION_ACCESS_DENIED);

                return context.Response.WriteAsJsonAsync(response);
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsJsonAsync(new ResponseErrorJson(ErrorMessages.VALIDATION_ACCESS_DENIED));
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddCors(options => options.AddPolicy(CORS_POLICY, policy =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Atrás do Caddy o TLS termina no proxy: a API só fala HTTP na rede interna do Docker.
// Redirecionar para HTTPS aqui geraria loop; quem redireciona é o proxy.
if (builder.Configuration.GetValue<bool>("ReverseProxy:Enabled"))
{
    var forwardedHeaders = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    };

    // A porta da API não é publicada no host — só o proxy alcança o container,
    // então não há rede intermediária para restringir.
    forwardedHeaders.KnownIPNetworks.Clear();
    forwardedHeaders.KnownProxies.Clear();

    app.UseForwardedHeaders(forwardedHeaders);
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(CORS_POLICY);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

DatabaseMigration.ExecuteMigrations(app.Services);
await DatabaseSeeder.Seed(app.Services);

app.Run();

public partial class Program
{
}
