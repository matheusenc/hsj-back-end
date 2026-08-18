namespace HospitalSaoJose.Domain.Repositories.Document;

public interface IDocumentUpdateOnlyRepository
{
    Task<Entities.Document?> GetById(Guid id);

    void Update(Entities.Document document);
}
