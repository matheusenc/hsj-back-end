using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Profile.Register;

public interface IRegisterProfileUseCase
{
    Task<ResponseRegisteredProfileJson> Execute(RequestProfileJson request);
}
