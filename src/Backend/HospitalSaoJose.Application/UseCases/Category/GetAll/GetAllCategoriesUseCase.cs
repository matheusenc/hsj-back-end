using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories.Category;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Category.GetAll;

public class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

    public GetAllCategoriesUseCase(ICategoryReadOnlyRepository categoryReadOnlyRepository) => _categoryReadOnlyRepository = categoryReadOnlyRepository;

    public async Task<ResponseCategoriesJson> Execute()
    {
        var categories = await _categoryReadOnlyRepository.GetAll();

        return new ResponseCategoriesJson { Categories = categories.Adapt<List<ResponseCategoryJson>>() };
    }
}
