namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseProfileJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystem { get; set; }
    public List<ResponseRoleSummaryJson> Roles { get; set; } = [];
}
