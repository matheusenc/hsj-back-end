using FluentValidation;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Category;

public class CategoryValidator : AbstractValidator<RequestCategoryJson>
{
    internal const string SLUG_PATTERN = "^[a-z0-9-]+$";

    public CategoryValidator()
    {
        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_CATEGORY_NAME_REQUIRED)
            .MaximumLength(150).WithMessage(ErrorMessages.VALIDATION_CATEGORY_NAME_MAX_LENGTH);

        RuleFor(request => request.Slug)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_CATEGORY_SLUG_REQUIRED)
            .MaximumLength(60).WithMessage(ErrorMessages.VALIDATION_CATEGORY_SLUG_MAX_LENGTH)
            .Matches(SLUG_PATTERN).WithMessage(ErrorMessages.VALIDATION_CATEGORY_SLUG_INVALID_FORMAT);
    }
}
