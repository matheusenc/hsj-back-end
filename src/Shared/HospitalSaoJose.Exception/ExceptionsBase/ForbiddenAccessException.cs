using System.Net;

namespace HospitalSaoJose.Exception.ExceptionsBase;

public sealed class ForbiddenAccessException : HospitalSaoJoseException
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;

    public override List<string> GetErrorMessages() => [Message];
}
