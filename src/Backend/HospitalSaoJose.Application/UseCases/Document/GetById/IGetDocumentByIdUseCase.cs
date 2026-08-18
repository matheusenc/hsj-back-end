using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Document.GetById;

public interface IGetDocumentByIdUseCase
{
    Task<ResponseDocumentJson> Execute(Guid id);
}
