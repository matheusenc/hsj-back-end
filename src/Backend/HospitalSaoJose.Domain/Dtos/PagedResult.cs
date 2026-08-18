namespace HospitalSaoJose.Domain.Dtos;

public sealed class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
