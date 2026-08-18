using HospitalSaoJose.Domain.Entities;

namespace HospitalSaoJose.Domain.Identity;

public interface ILoggedUser
{
    Task<User> Get();

    Guid GetUserId();
}
