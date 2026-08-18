namespace HospitalSaoJose.Domain.Entities;

public sealed class Document : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ExternalLink { get; set; }
    public DateOnly PublicationDate { get; set; }
    public DateOnly? PaymentDate { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid CreatedByUserId { get; set; }
}
