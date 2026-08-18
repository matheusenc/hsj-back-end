using System.Security.Claims;
using System.Text;
using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Security.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HospitalSaoJose.Infrastructure.Security.Tokens.Access;

internal sealed class JwtTokenHandler : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signingKey;

    public JwtTokenHandler(uint expirationTimeMinutes, string signingKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signingKey = signingKey;
    }

    public AccessToken Generate(User user, IEnumerable<string> permissions, bool isSuperAdmin)
    {
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id.ToString()) };

        claims.AddRange(permissions.Select(permission => new Claim(TokenClaims.Permission, permission)));

        if (isSuperAdmin)
            claims.Add(new Claim(TokenClaims.SuperAdmin, bool.TrueString));

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = expiresAtUtc,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var value = new JsonWebTokenHandler().CreateToken(tokenDescriptor);

        return new AccessToken(value, expiresAtUtc);
    }

    private SymmetricSecurityKey SecurityKey() => new(Encoding.UTF8.GetBytes(_signingKey));
}
