using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Role.Register;

public interface IRegisterRoleUseCase
{
    Task<ResponseRegisteredRoleJson> Execute(RequestRegisterRoleJson request);
}
