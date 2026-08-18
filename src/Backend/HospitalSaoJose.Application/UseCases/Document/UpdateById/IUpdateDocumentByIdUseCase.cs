using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.Document.UpdateById;

public interface IUpdateDocumentByIdUseCase
{
    Task Execute(Guid id, RequestDocumentJson request, DocumentFile? file);
}
