using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICategoryUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryUpdateOnlyRepository> _repository = new();

    public ICategoryUpdateOnlyRepositoryBuilder GetById(Category category)
    {
        _repository.Setup(repository => repository.GetById(category.Id)).ReturnsAsync(category);

        return this;
    }

    public ICategoryUpdateOnlyRepository Build() => _repository.Object;
}
