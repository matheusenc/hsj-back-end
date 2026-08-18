using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Role.GetAll;

public interface IGetAllRolesUseCase
{
    Task<ResponseRolesJson> Execute();
}
