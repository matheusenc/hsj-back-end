using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICategoryReadOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryReadOnlyRepository> _repository = new();

    public ICategoryReadOnlyRepositoryBuilder GetAll(List<Category> categories)
    {
        _repository.Setup(repository => repository.GetAll()).ReturnsAsync(categories);

        return this;
    }

    public ICategoryReadOnlyRepositoryBuilder GetById(Category category)
    {
        _repository.Setup(repository => repository.GetById(category.Id)).ReturnsAsync(category);

        return this;
    }

    public ICategoryReadOnlyRepositoryBuilder ExistActiveCategoryWithSlug(string slug)
    {
        _repository.Setup(repository => repository.ExistActiveCategoryWithSlug(slug)).ReturnsAsync(true);

        return this;
    }

    public ICategoryReadOnlyRepositoryBuilder ExistActiveCategoryWithSlugForOtherCategory(string slug)
    {
        _repository.Setup(repository => repository.ExistActiveCategoryWithSlugForOtherCategory(slug, It.IsAny<Guid>())).ReturnsAsync(true);

        return this;
    }

    public ICategoryReadOnlyRepository Build() => _repository.Object;
}
