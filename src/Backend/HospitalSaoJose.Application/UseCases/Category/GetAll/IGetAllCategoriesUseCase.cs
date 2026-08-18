using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.Category.GetAll;

public interface IGetAllCategoriesUseCase
{
    Task<ResponseCategoriesJson> Execute();
}
