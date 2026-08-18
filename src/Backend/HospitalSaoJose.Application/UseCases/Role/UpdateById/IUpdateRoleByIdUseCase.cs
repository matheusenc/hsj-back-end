using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.Role.UpdateById;

public interface IUpdateRoleByIdUseCase
{
    Task Execute(Guid id, RequestUpdateRoleJson request);
}
