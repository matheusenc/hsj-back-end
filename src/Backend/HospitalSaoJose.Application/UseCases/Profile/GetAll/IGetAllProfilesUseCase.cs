using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Profile.GetAll;

public interface IGetAllProfilesUseCase
{
    Task<ResponseProfilesJson> Execute();
}
