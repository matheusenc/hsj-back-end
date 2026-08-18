using FluentMigrator;

namespace HospitalSaoJose.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_DOCUMENTS, "Criando a tabela Documents")]
public class Version0000006 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Documents")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("Description").AsString(2000).NotNullable()
            .WithColumn("ExternalLink").AsString(2000).Nullable()
            .WithColumn("PublicationDate").AsDate().NotNullable()
            .WithColumn("PaymentDate").AsDate().Nullable()
            .WithColumn("OriginalFileName").AsString(255).NotNullable()
            .WithColumn("StoredFileName").AsString(100).NotNullable()
            .WithColumn("ContentType").AsString(100).NotNullable()
            .WithColumn("SizeInBytes").AsInt64().NotNullable()
            .WithColumn("CategoryId").AsGuid().NotNullable().ForeignKey("FK_Documents_Categories", "Categories", "Id")
            .WithColumn("CreatedByUserId").AsGuid().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedOn").AsDateTimeOffset().NotNullable();

        Create.Index("IX_Documents_PublicationDate").OnTable("Documents").OnColumn("PublicationDate").Descending();
    }
}
