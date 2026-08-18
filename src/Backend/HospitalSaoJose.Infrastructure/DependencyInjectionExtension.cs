using System.Reflection;
using FluentMigrator.Runner;
using HospitalSaoJose.Domain.Identity;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Category;
using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Domain.Repositories.Profile;
using HospitalSaoJose.Domain.Repositories.Role;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Domain.Security.PasswordHashing;
using HospitalSaoJose.Domain.Security.Tokens;
using HospitalSaoJose.Domain.Storage;
using HospitalSaoJose.Infrastructure.DataAccess;
using HospitalSaoJose.Infrastructure.DataAccess.Repositories;
using HospitalSaoJose.Infrastructure.Identity;
using HospitalSaoJose.Infrastructure.Security.PasswordHashing;
using HospitalSaoJose.Infrastructure.Security.Tokens.Access;
using HospitalSaoJose.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalSaoJose.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddRepositories();
            services.AddSecurity(configuration);
            services.AddStorage(configuration);
            services.AddMigrator(configuration);
        }

        private void AddDatabase(IConfiguration configuration)
        {
            services.AddDbContext<HospitalSaoJoseDbContext>(options => options.UseNpgsql(ConnectionString(configuration)));
        }

        private void AddRepositories()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

            services.AddScoped<IProfileReadOnlyRepository, ProfileRepository>();
            services.AddScoped<IProfileWriteOnlyRepository, ProfileRepository>();
            services.AddScoped<IProfileUpdateOnlyRepository, ProfileRepository>();

            services.AddScoped<IRoleReadOnlyRepository, RoleRepository>();
            services.AddScoped<IRoleWriteOnlyRepository, RoleRepository>();
            services.AddScoped<IRoleUpdateOnlyRepository, RoleRepository>();

            services.AddScoped<ICategoryReadOnlyRepository, CategoryRepository>();
            services.AddScoped<ICategoryWriteOnlyRepository, CategoryRepository>();
            services.AddScoped<ICategoryUpdateOnlyRepository, CategoryRepository>();

            services.AddScoped<IDocumentReadOnlyRepository, DocumentRepository>();
            services.AddScoped<IDocumentWriteOnlyRepository, DocumentRepository>();
            services.AddScoped<IDocumentUpdateOnlyRepository, DocumentRepository>();
        }

        private void AddSecurity(IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
            services.AddScoped<ILoggedUser, LoggedUser>();

            var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

            services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenHandler(expirationTimeMinutes, signingKey));
        }

        private void AddStorage(IConfiguration configuration)
        {
            var rootPath = configuration.GetValue<string>("FileStorage:RootPath");

            if (string.IsNullOrWhiteSpace(rootPath))
                rootPath = Path.Combine(AppContext.BaseDirectory, "storage");

            services.AddScoped<IFileStorageService>(_ => new LocalFileStorageService(rootPath));
        }

        private void AddMigrator(IConfiguration configuration)
        {
            services.AddFluentMigratorCore().ConfigureRunner(options => options
                .AddPostgres()
                .WithGlobalConnectionString(ConnectionString(configuration))
                .ScanIn(Assembly.Load("HospitalSaoJose.Infrastructure")).For.All());
        }
    }

    private static string ConnectionString(IConfiguration configuration) => configuration.GetConnectionString("DbConnection")!;
}
