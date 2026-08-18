using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.User.UpdateById;

public interface IUpdateUserByIdUseCase
{
    Task Execute(Guid id, RequestUpdateUserJson request);
}
