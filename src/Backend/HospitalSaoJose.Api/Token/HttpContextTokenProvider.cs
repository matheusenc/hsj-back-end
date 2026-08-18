using HospitalSaoJose.Domain.Security.Tokens;

namespace HospitalSaoJose.Api.Token;

internal sealed class HttpContextTokenProvider : IAccessTokenProvider
{
    private const string BEARER_PREFIX = "Bearer ";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public string GetToken()
    {
        var authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization.StartsWith(BEARER_PREFIX, StringComparison.OrdinalIgnoreCase)
            ? authorization[BEARER_PREFIX.Length..]
            : authorization;
    }
}
