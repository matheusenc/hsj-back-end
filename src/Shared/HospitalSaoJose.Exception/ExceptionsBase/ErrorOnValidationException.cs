using System.Net;

namespace HospitalSaoJose.Exception.ExceptionsBase;

public sealed class ErrorOnValidationException : HospitalSaoJoseException
{
    private readonly List<string> _errorMessages;

    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty) => _errorMessages = errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;

    public override List<string> GetErrorMessages() => _errorMessages;
}
