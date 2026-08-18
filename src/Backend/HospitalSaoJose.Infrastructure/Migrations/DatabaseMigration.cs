using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalSaoJose.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void ExecuteMigrations(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();
        runner.MigrateUp();
    }
}
