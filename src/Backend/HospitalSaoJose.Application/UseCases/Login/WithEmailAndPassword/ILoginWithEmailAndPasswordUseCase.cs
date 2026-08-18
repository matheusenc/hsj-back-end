using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    Task<ResponseTokensJson> Execute(RequestLoginJson request);
}
