namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseUserJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public ResponseProfileSummaryJson Profile { get; set; } = new();
}
