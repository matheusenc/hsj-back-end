namespace HospitalSaoJose.Domain.Repositories.Role;

public interface IRoleUpdateOnlyRepository
{
    Task<Entities.Role?> GetById(Guid id);

    void Update(Entities.Role role);
}
