using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Category;
using Microsoft.EntityFrameworkCore;

namespace HospitalSaoJose.Infrastructure.DataAccess.Repositories;

internal sealed class CategoryRepository : ICategoryReadOnlyRepository, ICategoryWriteOnlyRepository, ICategoryUpdateOnlyRepository
{
    private readonly HospitalSaoJoseDbContext _dbContext;

    public CategoryRepository(HospitalSaoJoseDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Category category) => await _dbContext.Categories.AddAsync(category);

    public async Task<List<Category>> GetAll()
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .Where(category => category.Active)
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name)
            .ToListAsync();
    }

    async Task<Category?> ICategoryReadOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Active && category.Id == id);
    }

    async Task<Category?> ICategoryUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(category => category.Active && category.Id == id);
    }

    public async Task<bool> ExistActiveCategoryWithSlug(string slug)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .AnyAsync(category => category.Active && category.Slug == slug);
    }

    public async Task<bool> ExistActiveCategoryWithSlugForOtherCategory(string slug, Guid categoryId)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .AnyAsync(category => category.Active && category.Slug == slug && category.Id != categoryId);
    }

    public void Update(Category category) => _dbContext.Categories.Update(category);
}
