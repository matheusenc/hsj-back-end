using FluentMigrator;

namespace HospitalSaoJose.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_CATEGORIES, "Criando a tabela Categories")]
public class Version0000005 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Categories")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(150).NotNullable()
            .WithColumn("Slug").AsString(60).NotNullable()
            .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedOn").AsDateTimeOffset().NotNullable();

        Execute.Sql("""CREATE UNIQUE INDEX "IX_Categories_Slug" ON "Categories" ("Slug") WHERE "Active" = true;""");
    }
}
