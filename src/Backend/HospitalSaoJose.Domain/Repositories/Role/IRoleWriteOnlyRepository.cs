namespace HospitalSaoJose.Domain.Repositories.Role;

public interface IRoleWriteOnlyRepository
{
    Task Add(Entities.Role role);
}
