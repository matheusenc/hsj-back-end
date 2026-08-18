namespace HospitalSaoJose.Communication.Requests;

public sealed class RequestDocumentJson
{
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ExternalLink { get; set; }
    public DateOnly PublicationDate { get; set; }
    public DateOnly? PaymentDate { get; set; }
}
