using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Category;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Category.Register;

public class RegisterCategoryUseCase : IRegisterCategoryUseCase
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCategoryUseCase(
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ICategoryWriteOnlyRepository categoryWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredCategoryJson> Execute(RequestCategoryJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var category = request.Adapt<Domain.Entities.Category>();

        await _categoryWriteOnlyRepository.Add(category);
        await _unitOfWork.Commit();

        return new ResponseRegisteredCategoryJson { Id = category.Id, Slug = category.Slug };
    }

    private async Task ValidateAndThrowOnFailures(RequestCategoryJson request)
    {
        var result = new CategoryValidator().Validate(request);

        var slugAlreadyExists = await _categoryReadOnlyRepository.ExistActiveCategoryWithSlug(request.Slug);
        if (slugAlreadyExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_CATEGORY_SLUG_ALREADY_EXISTS));

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
