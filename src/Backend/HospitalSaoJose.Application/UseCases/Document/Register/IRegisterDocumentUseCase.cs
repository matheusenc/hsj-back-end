using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Document.Register;

public interface IRegisterDocumentUseCase
{
    Task<ResponseRegisteredDocumentJson> Execute(RequestDocumentJson request, DocumentFile? file);
}
