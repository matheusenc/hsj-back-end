namespace HospitalSaoJose.Communication.Requests;

public sealed class RequestProfileJson
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Guid> RoleIds { get; set; } = [];
}
