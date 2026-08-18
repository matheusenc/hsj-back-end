namespace HospitalSaoJose.Domain.Repositories.Category;

public interface ICategoryReadOnlyRepository
{
    Task<List<Entities.Category>> GetAll();

    Task<Entities.Category?> GetById(Guid id);

    Task<bool> ExistActiveCategoryWithSlug(string slug);

    Task<bool> ExistActiveCategoryWithSlugForOtherCategory(string slug, Guid categoryId);
}
