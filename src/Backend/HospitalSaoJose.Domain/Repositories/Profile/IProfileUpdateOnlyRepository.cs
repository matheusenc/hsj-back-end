namespace HospitalSaoJose.Domain.Repositories.Profile;

public interface IProfileUpdateOnlyRepository
{
    Task<Entities.Profile?> GetById(Guid id);

    void Update(Entities.Profile profile);
}
