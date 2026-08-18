namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseRegisteredCategoryJson
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
}
