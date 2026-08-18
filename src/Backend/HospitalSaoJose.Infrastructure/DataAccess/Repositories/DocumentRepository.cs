using HospitalSaoJose.Domain.Dtos;
using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Extensions;
using HospitalSaoJose.Domain.Repositories.Document;
using Microsoft.EntityFrameworkCore;

namespace HospitalSaoJose.Infrastructure.DataAccess.Repositories;

internal sealed class DocumentRepository : IDocumentReadOnlyRepository, IDocumentWriteOnlyRepository, IDocumentUpdateOnlyRepository
{
    private readonly HospitalSaoJoseDbContext _dbContext;

    public DocumentRepository(HospitalSaoJoseDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Document document) => await _dbContext.Documents.AddAsync(document);

    public async Task<PagedResult<Document>> Filter(DocumentFilterDto filter)
    {
        var query = _dbContext.Documents
            .AsNoTracking()
            .Include(document => document.Category)
            .Where(document => document.Active && document.Category.Active);

        if (filter.CategorySlug.IsNotEmpty())
            query = query.Where(document => document.Category.Slug == filter.CategorySlug);

        if (filter.Title.IsNotEmpty())
            query = query.Where(document => EF.Functions.ILike(document.Title, $"%{filter.Title}%"));

        var totalCount = await query.CountAsync();

        var documents = await query
            .OrderByDescending(document => document.PublicationDate)
            .ThenByDescending(document => document.CreatedOn)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Document> { Items = documents, TotalCount = totalCount };
    }

    async Task<Document?> IDocumentReadOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .Include(document => document.Category)
            .FirstOrDefaultAsync(document => document.Active && document.Id == id);
    }

    async Task<Document?> IDocumentUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Documents
            .Include(document => document.Category)
            .FirstOrDefaultAsync(document => document.Active && document.Id == id);
    }

    public async Task<bool> ExistActiveDocumentInCategory(Guid categoryId)
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .AnyAsync(document => document.Active && document.CategoryId == categoryId);
    }

    public void Update(Document document) => _dbContext.Documents.Update(document);
}
