using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Profile;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IProfileWriteOnlyRepositoryBuilder
{
    private readonly Mock<IProfileWriteOnlyRepository> _repository = new();

    /// <summary>Entidade que o use case mandou persistir, para asserções sobre o que foi montado.</summary>
    public Profile? AddedProfile { get; private set; }

    public IProfileWriteOnlyRepositoryBuilder()
    {
        _repository
            .Setup(repository => repository.Add(It.IsAny<Profile>()))
            .Callback<Profile>(profile => AddedProfile = profile)
            .Returns(Task.CompletedTask);
    }

    public IProfileWriteOnlyRepository Build() => _repository.Object;
}
