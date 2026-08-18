using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace WebApi.Tests;

/// <summary>
/// Sobe um PostgreSQL real em container: as migrations e o seed rodam no startup da API,
/// então o teste exercita exatamente o mesmo caminho da produção.
/// Requer Docker disponível na máquina.
/// </summary>
public class HospitalSaoJoseApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public const string ADMIN_EMAIL = "admin@teste.com.br";
    public const string ADMIN_PASSWORD = "Admin@12345";

    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("hospitalsaojose")
        .Build();

    private readonly string _storageRootPath = Path.Combine(Path.GetTempPath(), $"hsj-tests-{Guid.CreateVersion7()}");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Tests")
            .ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DbConnection"] = _database.GetConnectionString(),
                ["Jwt:SigningKey"] = "chave-de-teste-com-tamanho-suficiente-para-hmac-sha256",
                ["Jwt:ExpirationTimeMinutes"] = "60",
                ["FileStorage:RootPath"] = _storageRootPath,
                ["Seed:AdminName"] = "Administrador",
                ["Seed:AdminEmail"] = ADMIN_EMAIL,
                ["Seed:AdminPassword"] = ADMIN_PASSWORD
            }));
    }

    async Task IAsyncLifetime.InitializeAsync() => await _database.StartAsync();

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _database.DisposeAsync();

        if (Directory.Exists(_storageRootPath))
            Directory.Delete(_storageRootPath, recursive: true);
    }
}
