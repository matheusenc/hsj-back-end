namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseTokensJson
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
}
