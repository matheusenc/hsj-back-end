namespace HospitalSaoJose.Application.UseCases.User.Deactivate;

public interface IDeactivateUserUseCase
{
    Task Execute(Guid id);
}
