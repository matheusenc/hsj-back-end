using HospitalSaoJose.Domain.Entities;

namespace HospitalSaoJose.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    AccessToken Generate(User user, IEnumerable<string> permissions, bool isSuperAdmin);
}
