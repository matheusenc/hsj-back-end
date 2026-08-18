namespace HospitalSaoJose.Application.UseCases.Document;

public sealed record DocumentDownload(Stream Content, string ContentType, string FileName);
