namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseLoggedUserJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ResponseProfileSummaryJson Profile { get; set; } = new();
    public List<string> Permissions { get; set; } = [];
}
