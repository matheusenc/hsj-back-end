using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.Profile.UpdateById;

public interface IUpdateProfileByIdUseCase
{
    Task Execute(Guid id, RequestProfileJson request);
}
