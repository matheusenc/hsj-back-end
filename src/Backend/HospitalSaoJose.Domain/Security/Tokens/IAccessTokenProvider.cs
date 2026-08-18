namespace HospitalSaoJose.Domain.Security.Tokens;

public interface IAccessTokenProvider
{
    string GetToken();
}
