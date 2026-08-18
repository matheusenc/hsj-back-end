using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Profile;
using Microsoft.EntityFrameworkCore;

namespace HospitalSaoJose.Infrastructure.DataAccess.Repositories;

internal sealed class ProfileRepository : IProfileReadOnlyRepository, IProfileWriteOnlyRepository, IProfileUpdateOnlyRepository
{
    private readonly HospitalSaoJoseDbContext _dbContext;

    public ProfileRepository(HospitalSaoJoseDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Profile profile) => await _dbContext.Profiles.AddAsync(profile);

    public async Task<List<Profile>> GetAll()
    {
        return await _dbContext.Profiles
            .AsNoTracking()
            .Include(profile => profile.ProfileRoles)
            .ThenInclude(profileRole => profileRole.Role)
            .Where(profile => profile.Active)
            .OrderBy(profile => profile.Name)
            .ToListAsync();
    }

    async Task<Profile?> IProfileReadOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Profiles
            .AsNoTracking()
            .Include(profile => profile.ProfileRoles)
            .ThenInclude(profileRole => profileRole.Role)
            .FirstOrDefaultAsync(profile => profile.Active && profile.Id == id);
    }

    async Task<Profile?> IProfileUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Profiles
            .Include(profile => profile.ProfileRoles)
            .FirstOrDefaultAsync(profile => profile.Active && profile.Id == id);
    }

    public async Task<bool> ExistActiveProfileWithName(string name)
    {
        return await _dbContext.Profiles
            .AsNoTracking()
            .AnyAsync(profile => profile.Active && profile.Name == name);
    }

    public async Task<bool> ExistActiveProfileWithNameForOtherProfile(string name, Guid profileId)
    {
        return await _dbContext.Profiles
            .AsNoTracking()
            .AnyAsync(profile => profile.Active && profile.Name == name && profile.Id != profileId);
    }

    public void Update(Profile profile) => _dbContext.Profiles.Update(profile);
}
