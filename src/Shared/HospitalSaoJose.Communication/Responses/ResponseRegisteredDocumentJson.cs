namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseRegisteredDocumentJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
}
