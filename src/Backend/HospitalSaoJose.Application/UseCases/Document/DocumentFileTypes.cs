using HospitalSaoJose.Application.Extensions;

namespace HospitalSaoJose.Application.UseCases.Document;

/// <summary>
/// Trecho de bytes que um formato coloca sempre na mesma posição do arquivo.
/// </summary>
internal readonly record struct FileSignature(int Offset, byte[] Bytes);

internal sealed record AcceptedFileType(string ContentType, FileSignature[] Signatures);

/// <summary>
/// Lista de permissão dos formatos aceitos no upload de documentos.
///
/// É lista de permissão, e não de bloqueio: com bloqueio, todo formato
/// perigoso que aparecer depois entra sozinho até alguém lembrar de barrá-lo.
/// Ficam de fora, por decisão: <c>.svg</c> (é XML com script executável, ainda
/// que pareça imagem) e qualquer executável.
///
/// O <c>ContentType</c> sai daqui, e nunca do cabeçalho que o navegador manda
/// junto do upload — aquele é escolhido pelo cliente.
/// </summary>
internal static class DocumentFileTypes
{
    // Office moderno e OpenDocument são pacotes ZIP; o Office antigo é OLE2.
    private static readonly FileSignature[] Zip =
    [
        new(0, [0x50, 0x4B, 0x03, 0x04]),
        new(0, [0x50, 0x4B, 0x05, 0x06]),
        new(0, [0x50, 0x4B, 0x07, 0x08])
    ];

    private static readonly FileSignature[] Ole = [new(0, [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1])];

    /// <summary>Texto puro não tem assinatura: a extensão decide sozinha.</summary>
    private static readonly FileSignature[] Nenhuma = [];

    private static readonly Dictionary<string, AcceptedFileType> PorExtensao = new(StringComparer.OrdinalIgnoreCase)
    {
        [".pdf"] = new("application/pdf", [new(0, [0x25, 0x50, 0x44, 0x46])]),

        [".doc"] = new("application/msword", Ole),
        [".docx"] = new("application/vnd.openxmlformats-officedocument.wordprocessingml.document", Zip),
        [".odt"] = new("application/vnd.oasis.opendocument.text", Zip),
        [".rtf"] = new("application/rtf", [new(0, [0x7B, 0x5C, 0x72, 0x74, 0x66])]),
        [".txt"] = new("text/plain", Nenhuma),

        [".xls"] = new("application/vnd.ms-excel", Ole),
        [".xlsx"] = new("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Zip),
        [".ods"] = new("application/vnd.oasis.opendocument.spreadsheet", Zip),
        [".csv"] = new("text/csv", Nenhuma),

        [".ppt"] = new("application/vnd.ms-powerpoint", Ole),
        [".pptx"] = new("application/vnd.openxmlformats-officedocument.presentationml.presentation", Zip),
        [".odp"] = new("application/vnd.oasis.opendocument.presentation", Zip),

        [".jpg"] = new("image/jpeg", [new(0, [0xFF, 0xD8, 0xFF])]),
        [".jpeg"] = new("image/jpeg", [new(0, [0xFF, 0xD8, 0xFF])]),
        [".png"] = new("image/png", [new(0, [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A])]),
        [".gif"] = new("image/gif", [new(0, [0x47, 0x49, 0x46, 0x38])]),
        // No WEBP os quatro primeiros bytes são "RIFF", comuns a outros
        // formatos; quem identifica é o "WEBP" no oitavo byte.
        [".webp"] = new("image/webp", [new(8, [0x57, 0x45, 0x42, 0x50])]),
        [".tif"] = new("image/tiff", [new(0, [0x49, 0x49, 0x2A, 0x00]), new(0, [0x4D, 0x4D, 0x00, 0x2A])]),
        [".tiff"] = new("image/tiff", [new(0, [0x49, 0x49, 0x2A, 0x00]), new(0, [0x4D, 0x4D, 0x00, 0x2A])]),

        [".zip"] = new("application/zip", Zip)
    };

    internal static AcceptedFileType? Find(string extension) =>
        PorExtensao.TryGetValue(extension, out var tipo) ? tipo : null;

    /// <summary>
    /// Confere se os primeiros bytes batem com o que a extensão promete. É o
    /// que impede um executável renomeado para <c>.pdf</c> de ser aceito.
    /// Formato sem assinatura conhecida passa direto.
    /// </summary>
    internal static bool MatchesContent(this AcceptedFileType type, Stream content) =>
        type.Signatures.Length == 0 ||
        type.Signatures.Any(assinatura => content.StartsWith(assinatura.Offset, assinatura.Bytes));
}
