namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseDocumentJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ExternalLink { get; set; }
    public DateOnly PublicationDate { get; set; }
    public DateOnly? PaymentDate { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public string DownloadUrl { get; set; } = string.Empty;
    public ResponseCategoryJson Category { get; set; } = new();
}
