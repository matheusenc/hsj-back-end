using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Profile;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IProfileReadOnlyRepositoryBuilder
{
    private readonly Mock<IProfileReadOnlyRepository> _repository = new();

    public IProfileReadOnlyRepositoryBuilder GetAll(List<Profile> profiles)
    {
        _repository.Setup(repository => repository.GetAll()).ReturnsAsync(profiles);

        return this;
    }

    public IProfileReadOnlyRepositoryBuilder GetById(Profile profile)
    {
        _repository.Setup(repository => repository.GetById(profile.Id)).ReturnsAsync(profile);

        return this;
    }

    public IProfileReadOnlyRepositoryBuilder ExistActiveProfileWithName(string name)
    {
        _repository.Setup(repository => repository.ExistActiveProfileWithName(name)).ReturnsAsync(true);

        return this;
    }

    public IProfileReadOnlyRepositoryBuilder ExistActiveProfileWithNameForOtherProfile(string name)
    {
        _repository.Setup(repository => repository.ExistActiveProfileWithNameForOtherProfile(name, It.IsAny<Guid>())).ReturnsAsync(true);

        return this;
    }

    public IProfileReadOnlyRepository Build() => _repository.Object;
}
