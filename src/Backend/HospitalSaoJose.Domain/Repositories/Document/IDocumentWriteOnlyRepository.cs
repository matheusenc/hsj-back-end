namespace HospitalSaoJose.Domain.Repositories.Document;

public interface IDocumentWriteOnlyRepository
{
    Task Add(Entities.Document document);
}
