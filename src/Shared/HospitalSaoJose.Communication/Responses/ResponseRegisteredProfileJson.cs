namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseRegisteredProfileJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
