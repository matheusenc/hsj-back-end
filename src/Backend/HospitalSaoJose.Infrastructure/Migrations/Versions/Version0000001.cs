using FluentMigrator;

namespace HospitalSaoJose.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_ROLES, "Criando a tabela Roles")]
public class Version0000001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Roles")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Key").AsString(100).NotNullable()
            .WithColumn("Name").AsString(150).NotNullable()
            .WithColumn("Description").AsString(500).NotNullable().WithDefaultValue(string.Empty)
            .WithColumn("IsSystem").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedOn").AsDateTimeOffset().NotNullable();

        Execute.Sql("""CREATE UNIQUE INDEX "IX_Roles_Key" ON "Roles" ("Key") WHERE "Active" = true;""");
    }
}
