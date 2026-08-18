namespace HospitalSaoJose.Application.UseCases.Role.DeleteById;

public interface IDeleteRoleByIdUseCase
{
    Task Execute(Guid id);
}
