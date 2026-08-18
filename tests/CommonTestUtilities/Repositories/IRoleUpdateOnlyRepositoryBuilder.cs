using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Role;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IRoleUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IRoleUpdateOnlyRepository> _repository = new();

    public IRoleUpdateOnlyRepositoryBuilder GetById(Role role)
    {
        _repository.Setup(repository => repository.GetById(role.Id)).ReturnsAsync(role);

        return this;
    }

    public IRoleUpdateOnlyRepository Build() => _repository.Object;
}
