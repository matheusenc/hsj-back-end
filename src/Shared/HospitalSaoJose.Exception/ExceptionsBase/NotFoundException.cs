using System.Net;

namespace HospitalSaoJose.Exception.ExceptionsBase;

public sealed class NotFoundException : HospitalSaoJoseException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;

    public override List<string> GetErrorMessages() => [Message];
}
