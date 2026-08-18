using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IUserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository = new();

    public IUserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }

    public IUserUpdateOnlyRepository Build() => _repository.Object;
}
