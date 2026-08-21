using Ganss.Xss;

namespace HospitalSaoJose.Application.UseCases.Document;

/// <summary>
/// A descrição é escrita no painel com formatação e exibida no site, então é
/// HTML vindo de fora. Higienizar no navegador não protege nada: o painel não é
/// o único caminho até a API — basta um POST direto. Por isso a limpeza acontece
/// aqui, e o que vai para o banco já está limpo. Assim nenhuma tela precisa
/// lembrar de sanitizar na leitura.
///
/// A lista é de permissão: o que não estiver nela é removido.
/// </summary>
internal static class DocumentDescriptionSanitizer
{
    private static readonly HtmlSanitizer Sanitizer = Build();

    private static HtmlSanitizer Build()
    {
        var sanitizer = new HtmlSanitizer();

        sanitizer.AllowedTags.Clear();
        foreach (var tag in new[] { "p", "br", "strong", "b", "em", "i", "u", "ul", "ol", "li", "a" })
            sanitizer.AllowedTags.Add(tag);

        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedAttributes.Add("href");
        sanitizer.AllowedAttributes.Add("title");

        // Sem esquema liberado não há `javascript:` nem `data:` num href.
        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.Add("http");
        sanitizer.AllowedSchemes.Add("https");
        sanitizer.AllowedSchemes.Add("mailto");

        sanitizer.AllowedCssProperties.Clear();
        sanitizer.KeepChildNodes = true;

        // Quem decide como o link abre é o servidor, não quem escreveu o texto:
        // sem noopener, a página aberta ganha acesso ao window desta.
        sanitizer.PostProcessNode += (_, evento) =>
        {
            if (evento.Node is AngleSharp.Html.Dom.IHtmlAnchorElement âncora)
            {
                âncora.SetAttribute("target", "_blank");
                âncora.SetAttribute("rel", "noopener noreferrer");
            }
        };

        return sanitizer;
    }

    internal static string Sanitize(string description) => Sanitizer.Sanitize(description).Trim();

    /// <summary>
    /// Texto sem marcação, para onde a descrição aparece fora de uma página HTML
    /// e para medir o tamanho real do que foi escrito.
    /// </summary>
    internal static string ToPlainText(string description)
    {
        var semMarcacao = new HtmlSanitizer();
        semMarcacao.AllowedTags.Clear();
        semMarcacao.AllowedAttributes.Clear();
        semMarcacao.KeepChildNodes = true;

        return System.Net.WebUtility.HtmlDecode(semMarcacao.Sanitize(description)).Trim();
    }
}
