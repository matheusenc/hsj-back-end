namespace HospitalSaoJose.Application.UseCases.Document;

/// <summary>
/// Transporta o arquivo enviado sem acoplar a Application ao ASP.NET (IFormFile fica na Api).
/// </summary>
public sealed record DocumentFile(Stream Content, string FileName, long SizeInBytes)
{
    /// <summary>
    /// Extensão com o ponto, como veio no nome enviado. A comparação com a lista
    /// de tipos aceitos ignora a caixa, então <c>.PDF</c> vale tanto quanto
    /// <c>.pdf</c>. Sem extensão, devolve string vazia — e nenhuma entrada da
    /// lista casa com isso.
    /// </summary>
    internal string Extension() => Path.GetExtension(FileName);
}
