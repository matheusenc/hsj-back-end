namespace HospitalSaoJose.Domain.Repositories.Profile;

public interface IProfileReadOnlyRepository
{
    Task<List<Entities.Profile>> GetAll();

    Task<Entities.Profile?> GetById(Guid id);

    Task<bool> ExistActiveProfileWithName(string name);

    Task<bool> ExistActiveProfileWithNameForOtherProfile(string name, Guid profileId);
}
