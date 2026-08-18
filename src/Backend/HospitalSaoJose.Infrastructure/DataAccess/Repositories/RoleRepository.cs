using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Role;
using Microsoft.EntityFrameworkCore;

namespace HospitalSaoJose.Infrastructure.DataAccess.Repositories;

internal sealed class RoleRepository : IRoleReadOnlyRepository, IRoleWriteOnlyRepository, IRoleUpdateOnlyRepository
{
    private readonly HospitalSaoJoseDbContext _dbContext;

    public RoleRepository(HospitalSaoJoseDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Role role) => await _dbContext.Roles.AddAsync(role);

    public async Task<List<Role>> GetAll()
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .Include(role => role.ProfileRoles)
            .ThenInclude(profileRole => profileRole.Profile)
            .Where(role => role.Active)
            .OrderBy(role => role.Key)
            .ToListAsync();
    }

    async Task<Role?> IRoleReadOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .Include(role => role.ProfileRoles)
            .ThenInclude(profileRole => profileRole.Profile)
            .FirstOrDefaultAsync(role => role.Active && role.Id == id);
    }

    async Task<Role?> IRoleUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(role => role.Active && role.Id == id);
    }

    public async Task<List<Role>> GetByIds(IList<Guid> ids)
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .Where(role => role.Active && ids.Contains(role.Id))
            .ToListAsync();
    }

    public async Task<bool> ExistActiveRoleWithKey(string key)
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .AnyAsync(role => role.Active && role.Key == key);
    }

    public async Task<bool> ExistProfileUsingRole(Guid roleId)
    {
        return await _dbContext.ProfileRoles
            .AsNoTracking()
            .AnyAsync(profileRole => profileRole.RoleId == roleId && profileRole.Profile.Active);
    }

    public void Update(Role role) => _dbContext.Roles.Update(role);
}
