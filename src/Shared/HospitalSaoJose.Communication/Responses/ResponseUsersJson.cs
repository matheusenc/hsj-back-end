namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseUsersJson
{
    public List<ResponseUserJson> Users { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
