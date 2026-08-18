using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Profile.GetById;

public interface IGetProfileByIdUseCase
{
    Task<ResponseProfileJson> Execute(Guid id);
}
