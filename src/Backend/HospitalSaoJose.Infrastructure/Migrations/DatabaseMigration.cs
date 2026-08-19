using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalSaoJose.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void ExecuteMigrations(IServiceProvider serviceProvider)
    {
        // O IMigrationRunner é scoped, e aqui chega o provider raiz. Resolver
        // direto dele quebra a validação de escopo que o ASP.NET liga em
        // Development, e em Production só não quebra porque a validação está
        // desligada — o runner e a conexão dele ficariam presos ao container
        // raiz pelo resto da vida do processo.
        using var scope = serviceProvider.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();
        runner.MigrateUp();
    }
}
