namespace HospitalSaoJose.Domain.Repositories.Role;

public interface IRoleReadOnlyRepository
{
    Task<List<Entities.Role>> GetAll();

    Task<Entities.Role?> GetById(Guid id);

    Task<List<Entities.Role>> GetByIds(IList<Guid> ids);

    Task<bool> ExistActiveRoleWithKey(string key);

    Task<bool> ExistProfileUsingRole(Guid roleId);
}
