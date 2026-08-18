using FluentMigrator;

namespace HospitalSaoJose.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USERS, "Criando a tabela Users")]
public class Version0000004 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("ProfileId").AsGuid().NotNullable().ForeignKey("FK_Users_Profiles", "Profiles", "Id")
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedOn").AsDateTimeOffset().NotNullable();

        Execute.Sql("""CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email") WHERE "Active" = true;""");
    }
}
