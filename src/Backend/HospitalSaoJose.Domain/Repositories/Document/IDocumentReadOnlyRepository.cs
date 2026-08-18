using HospitalSaoJose.Domain.Dtos;

namespace HospitalSaoJose.Domain.Repositories.Document;

public interface IDocumentReadOnlyRepository
{
    Task<PagedResult<Entities.Document>> Filter(DocumentFilterDto filter);

    Task<Entities.Document?> GetById(Guid id);

    Task<bool> ExistActiveDocumentInCategory(Guid categoryId);
}
