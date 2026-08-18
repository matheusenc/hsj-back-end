namespace HospitalSaoJose.Communication.Responses;

public sealed class ResponseErrorJson
{
    public List<string> Errors { get; private set; }
    public bool AccessTokenExpired { get; private set; }

    public ResponseErrorJson(List<string> errors) => Errors = errors;

    public ResponseErrorJson(string error) => Errors = [error];

    public ResponseErrorJson(string error, bool accessTokenExpired)
    {
        Errors = [error];
        AccessTokenExpired = accessTokenExpired;
    }
}
