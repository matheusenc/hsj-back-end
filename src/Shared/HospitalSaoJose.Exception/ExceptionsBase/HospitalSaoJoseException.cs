using System.Net;

namespace HospitalSaoJose.Exception.ExceptionsBase;

public abstract class HospitalSaoJoseException : System.Exception
{
    protected HospitalSaoJoseException(string message) : base(message)
    {
    }

    public abstract HttpStatusCode GetStatusCode();

    public abstract List<string> GetErrorMessages();
}
