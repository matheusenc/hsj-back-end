namespace HospitalSaoJose.Domain.Entities;

public sealed class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public ICollection<Document> Documents { get; set; } = [];
}
