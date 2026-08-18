using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Category.Register;

public interface IRegisterCategoryUseCase
{
    Task<ResponseRegisteredCategoryJson> Execute(RequestCategoryJson request);
}
