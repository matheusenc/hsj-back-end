using HospitalSaoJose.Domain.Dtos;
using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();

    public IUserReadOnlyRepositoryBuilder ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);

        return this;
    }

    public IUserReadOnlyRepositoryBuilder ExistActiveUserWithEmailForOtherUser(string email)
    {
        _repository.Setup(repository => repository.ExistActiveUserWithEmailForOtherUser(email, It.IsAny<Guid>())).ReturnsAsync(true);

        return this;
    }

    public IUserReadOnlyRepositoryBuilder ExistActiveUserWithProfile(Guid profileId)
    {
        _repository.Setup(repository => repository.ExistActiveUserWithProfile(profileId)).ReturnsAsync(true);

        return this;
    }

    public IUserReadOnlyRepositoryBuilder GetByEmail(User user)
    {
        _repository.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepositoryBuilder Filter(List<User> users, int totalCount)
    {
        _repository
            .Setup(repository => repository.Filter(It.IsAny<UserFilterDto>()))
            .ReturnsAsync(new PagedResult<User> { Items = users, TotalCount = totalCount });

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}
