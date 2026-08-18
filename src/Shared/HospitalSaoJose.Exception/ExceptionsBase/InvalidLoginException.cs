using System.Net;

namespace HospitalSaoJose.Exception.ExceptionsBase;

public sealed class InvalidLoginException : HospitalSaoJoseException
{
    public InvalidLoginException() : base(ErrorMessages.VALIDATION_LOGIN_INVALID)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;

    public override List<string> GetErrorMessages() => [Message];
}
