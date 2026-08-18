using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Identity;
using HospitalSaoJose.Domain.Security.Tokens;
using HospitalSaoJose.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HospitalSaoJose.Infrastructure.Identity;

internal sealed class LoggedUser : ILoggedUser
{
    private readonly HospitalSaoJoseDbContext _dbContext;
    private readonly IAccessTokenProvider _accessTokenProvider;

    public LoggedUser(HospitalSaoJoseDbContext dbContext, IAccessTokenProvider accessTokenProvider)
    {
        _dbContext = dbContext;
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<User> Get()
    {
        var userId = GetUserId();

        return await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Profile)
            .ThenInclude(profile => profile.ProfileRoles)
            .ThenInclude(profileRole => profileRole.Role)
            .FirstAsync(user => user.Active && user.Id == userId);
    }

    public Guid GetUserId()
    {
        var token = _accessTokenProvider.GetToken();

        var jsonWebToken = new JsonWebTokenHandler().ReadJsonWebToken(token);

        return Guid.Parse(jsonWebToken.Subject);
    }
}
