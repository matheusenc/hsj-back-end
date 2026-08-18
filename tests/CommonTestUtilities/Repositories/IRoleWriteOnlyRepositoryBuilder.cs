using HospitalSaoJose.Domain.Repositories.Role;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class IRoleWriteOnlyRepositoryBuilder
{
    public static IRoleWriteOnlyRepository Build() => new Mock<IRoleWriteOnlyRepository>().Object;
}
