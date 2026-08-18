namespace HospitalSaoJose.Application.UseCases.Document;

/// <summary>
/// Transporta o arquivo enviado sem acoplar a Application ao ASP.NET (IFormFile fica na Api).
/// </summary>
public sealed record DocumentFile(Stream Content, string FileName, long SizeInBytes);
