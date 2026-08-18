using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Profile;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IProfileUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IProfileUpdateOnlyRepository> _repository = new();

    public IProfileUpdateOnlyRepositoryBuilder GetById(Profile profile)
    {
        _repository.Setup(repository => repository.GetById(profile.Id)).ReturnsAsync(profile);

        return this;
    }

    public IProfileUpdateOnlyRepository Build() => _repository.Object;
}
