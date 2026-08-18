using HospitalSaoJose.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class IUnitOfWorkBuilder
{
    public static IUnitOfWork Build() => new Mock<IUnitOfWork>().Object;
}
