namespace HospitalSaoJose.Communication.Requests;

public sealed class RequestChangePasswordJson
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
