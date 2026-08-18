using HospitalSaoJose.Domain.Dtos;

namespace HospitalSaoJose.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);

    Task<bool> ExistActiveUserWithEmailForOtherUser(string email, Guid userId);

    Task<bool> ExistActiveUserWithProfile(Guid profileId);

    Task<Entities.User?> GetByEmail(string email);

    Task<Entities.User?> GetById(Guid id);

    Task<PagedResult<Entities.User>> Filter(UserFilterDto filter);
}
