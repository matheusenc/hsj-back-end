using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Role;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IRoleReadOnlyRepositoryBuilder
{
    private readonly Mock<IRoleReadOnlyRepository> _repository = new();

    public IRoleReadOnlyRepositoryBuilder GetAll(List<Role> roles)
    {
        _repository.Setup(repository => repository.GetAll()).ReturnsAsync(roles);

        return this;
    }

    public IRoleReadOnlyRepositoryBuilder GetById(Role role)
    {
        _repository.Setup(repository => repository.GetById(role.Id)).ReturnsAsync(role);

        return this;
    }

    public IRoleReadOnlyRepositoryBuilder GetByIds(List<Role> roles)
    {
        _repository.Setup(repository => repository.GetByIds(It.IsAny<IList<Guid>>())).ReturnsAsync(roles);

        return this;
    }

    public IRoleReadOnlyRepositoryBuilder ExistActiveRoleWithKey(string key)
    {
        _repository.Setup(repository => repository.ExistActiveRoleWithKey(key)).ReturnsAsync(true);

        return this;
    }

    public IRoleReadOnlyRepositoryBuilder ExistProfileUsingRole(Guid roleId)
    {
        _repository.Setup(repository => repository.ExistProfileUsingRole(roleId)).ReturnsAsync(true);

        return this;
    }

    public IRoleReadOnlyRepository Build() => _repository.Object;
}
