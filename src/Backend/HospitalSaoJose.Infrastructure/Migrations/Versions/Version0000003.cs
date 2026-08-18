using FluentMigrator;

namespace HospitalSaoJose.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_PROFILE_ROLES, "Criando a tabela de junção ProfileRoles")]
public class Version0000003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("ProfileRoles")
            .WithColumn("ProfileId").AsGuid().NotNullable().ForeignKey("FK_ProfileRoles_Profiles", "Profiles", "Id").OnDelete(System.Data.Rule.Cascade)
            .WithColumn("RoleId").AsGuid().NotNullable().ForeignKey("FK_ProfileRoles_Roles", "Roles", "Id").OnDelete(System.Data.Rule.Cascade);

        Create.PrimaryKey("PK_ProfileRoles").OnTable("ProfileRoles").Columns("ProfileId", "RoleId");
    }
}
