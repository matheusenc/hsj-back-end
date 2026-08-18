using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.User.Logged;

public interface IGetLoggedUserUseCase
{
    Task<ResponseLoggedUserJson> Execute();
}
