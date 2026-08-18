using HospitalSaoJose.Domain.Dtos;
using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Extensions;
using HospitalSaoJose.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace HospitalSaoJose.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly HospitalSaoJoseDbContext _dbContext;

    public UserRepository(HospitalSaoJoseDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Active && user.Email == email);
    }

    public async Task<bool> ExistActiveUserWithEmailForOtherUser(string email, Guid userId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Active && user.Email == email && user.Id != userId);
    }

    public async Task<bool> ExistActiveUserWithProfile(Guid profileId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Active && user.ProfileId == profileId);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Profile)
            .ThenInclude(profile => profile.ProfileRoles)
            .ThenInclude(profileRole => profileRole.Role)
            .FirstOrDefaultAsync(user => user.Active && user.Email == email);
    }

    async Task<User?> IUserReadOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Profile)
            .ThenInclude(profile => profile.ProfileRoles)
            .ThenInclude(profileRole => profileRole.Role)
            .FirstOrDefaultAsync(user => user.Active && user.Id == id);
    }

    async Task<User?> IUserUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Active && user.Id == id);
    }

    public async Task<PagedResult<User>> Filter(UserFilterDto filter)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Profile)
            .Where(user => user.Active);

        if (filter.Name.IsNotEmpty())
            query = query.Where(user => EF.Functions.ILike(user.Name, $"%{filter.Name}%"));

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(user => user.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<User> { Items = users, TotalCount = totalCount };
    }

    public void Update(User user) => _dbContext.Users.Update(user);
}
