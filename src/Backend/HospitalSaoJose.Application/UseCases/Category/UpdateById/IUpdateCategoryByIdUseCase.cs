using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.Category.UpdateById;

public interface IUpdateCategoryByIdUseCase
{
    Task Execute(Guid id, RequestCategoryJson request);
}
