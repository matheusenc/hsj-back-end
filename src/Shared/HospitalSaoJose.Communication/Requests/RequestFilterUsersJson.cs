namespace HospitalSaoJose.Communication.Requests;

public sealed class RequestFilterUsersJson
{
    public string? Name { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
