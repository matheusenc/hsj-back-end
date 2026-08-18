using FluentMigrator;

namespace HospitalSaoJose.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_PROFILES, "Criando a tabela Profiles")]
public class Version0000002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Profiles")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Description").AsString(500).NotNullable().WithDefaultValue(string.Empty)
            .WithColumn("IsSystem").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedOn").AsDateTimeOffset().NotNullable();

        Execute.Sql("""CREATE UNIQUE INDEX "IX_Profiles_Name" ON "Profiles" ("Name") WHERE "Active" = true;""");
    }
}
