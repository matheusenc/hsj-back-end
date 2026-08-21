using FluentMigrator;

namespace HospitalSaoJose.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.DOCUMENTS_DESCRIPTION_AS_HTML, "Descrição do documento passa a guardar HTML")]
public class Version0000007 : ForwardOnlyMigration
{
    public override void Up()
    {
        // 8000 porque agora cabe marcação junto do texto: um link sozinho gasta
        // uns 60 caracteres só de tag.
        Alter.Table("Documents")
            .AlterColumn("Description").AsString(8000).NotNullable();

        // As descrições existentes são texto puro. Sem converter, elas até
        // continuariam legíveis, mas as quebras de linha sumiriam ao serem
        // exibidas como HTML — e um `&` ou `<` no texto viraria marcação
        // quebrada. A ordem importa: escapar primeiro, converter depois.
        Execute.Sql("""
            UPDATE "Documents"
            SET "Description" = '<p>' || replace(
                                          replace(
                                            replace(
                                              replace(
                                                replace("Description", '&', '&amp;'),
                                              '<', '&lt;'),
                                            '>', '&gt;'),
                                          E'\r\n', '<br>'),
                                        E'\n', '<br>') || '</p>'
            WHERE "Description" <> '';
            """);
    }
}
