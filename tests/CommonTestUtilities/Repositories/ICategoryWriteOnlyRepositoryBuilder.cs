using HospitalSaoJose.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class ICategoryWriteOnlyRepositoryBuilder
{
    public static ICategoryWriteOnlyRepository Build() => new Mock<ICategoryWriteOnlyRepository>().Object;
}
