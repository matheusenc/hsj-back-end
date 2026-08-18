namespace HospitalSaoJose.Domain.Repositories.Profile;

public interface IProfileWriteOnlyRepository
{
    Task Add(Entities.Profile profile);
}
