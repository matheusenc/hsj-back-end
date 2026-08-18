namespace HospitalSaoJose.Application.UseCases.Document.DeleteById;

public interface IDeleteDocumentByIdUseCase
{
    Task Execute(Guid id);
}
