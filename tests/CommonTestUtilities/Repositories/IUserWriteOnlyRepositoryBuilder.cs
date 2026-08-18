using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IUserWriteOnlyRepositoryBuilder
{
    private readonly Mock<IUserWriteOnlyRepository> _repository = new();

    /// <summary>Entidade que o use case mandou persistir, para asserções sobre o que foi montado.</summary>
    public User? AddedUser { get; private set; }

    public IUserWriteOnlyRepositoryBuilder()
    {
        _repository
            .Setup(repository => repository.Add(It.IsAny<User>()))
            .Callback<User>(user => AddedUser = user)
            .Returns(Task.CompletedTask);
    }

    public IUserWriteOnlyRepository Build() => _repository.Object;
}
