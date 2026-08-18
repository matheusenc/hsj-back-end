namespace HospitalSaoJose.Domain.Dtos;

public sealed class UserFilterDto
{
    public string? Name { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
