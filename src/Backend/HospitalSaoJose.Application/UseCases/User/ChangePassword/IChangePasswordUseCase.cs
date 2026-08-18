using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
