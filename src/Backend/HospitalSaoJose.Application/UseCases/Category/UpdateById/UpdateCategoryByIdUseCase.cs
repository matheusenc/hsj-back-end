using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Category;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Category.UpdateById;

public class UpdateCategoryByIdUseCase : IUpdateCategoryByIdUseCase
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ICategoryUpdateOnlyRepository _categoryUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryByIdUseCase(
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ICategoryUpdateOnlyRepository categoryUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _categoryUpdateOnlyRepository = categoryUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestCategoryJson request)
    {
        var category = await _categoryUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.CATEGORY_NOT_FOUND);

        await ValidateAndThrowOnFailures(request, id);

        category.Name = request.Name;
        category.Slug = request.Slug;
        category.DisplayOrder = request.DisplayOrder;

        _categoryUpdateOnlyRepository.Update(category);
        await _unitOfWork.Commit();
    }

    private async Task ValidateAndThrowOnFailures(RequestCategoryJson request, Guid categoryId)
    {
        var result = new CategoryValidator().Validate(request);

        var slugAlreadyExists = await _categoryReadOnlyRepository.ExistActiveCategoryWithSlugForOtherCategory(request.Slug, categoryId);
        if (slugAlreadyExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_CATEGORY_SLUG_ALREADY_EXISTS));

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
