namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseRoleJson
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystem { get; set; }
    public List<ResponseProfileSummaryJson> Profiles { get; set; } = [];
}
