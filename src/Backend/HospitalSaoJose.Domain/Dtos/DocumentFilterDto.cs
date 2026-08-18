namespace HospitalSaoJose.Domain.Dtos;

public sealed class DocumentFilterDto
{
    public string? CategorySlug { get; set; }
    public string? Title { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
