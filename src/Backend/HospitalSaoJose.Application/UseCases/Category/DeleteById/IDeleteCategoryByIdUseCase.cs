namespace HospitalSaoJose.Application.UseCases.Category.DeleteById;

public interface IDeleteCategoryByIdUseCase
{
    Task Execute(Guid id);
}
