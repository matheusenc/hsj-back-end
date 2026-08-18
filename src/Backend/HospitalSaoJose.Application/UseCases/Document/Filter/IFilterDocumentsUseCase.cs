using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Document.Filter;

public interface IFilterDocumentsUseCase
{
    Task<ResponseDocumentsJson> Execute(RequestFilterDocumentsJson request);
}
