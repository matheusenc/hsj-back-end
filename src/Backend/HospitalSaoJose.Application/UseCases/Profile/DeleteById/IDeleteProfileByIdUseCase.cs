namespace HospitalSaoJose.Application.UseCases.Profile.DeleteById;

public interface IDeleteProfileByIdUseCase
{
    Task Execute(Guid id);
}
