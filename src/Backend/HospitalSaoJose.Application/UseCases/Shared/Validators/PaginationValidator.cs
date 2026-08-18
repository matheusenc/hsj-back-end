using FluentValidation;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Shared.Validators;

public static class PaginationValidator
{
    internal const int MAXIMUM_PAGE_SIZE = 100;

    extension<TRequest>(IRuleBuilderInitial<TRequest, int> ruleBuilder)
    {
        internal IRuleBuilderOptions<TRequest, int> Page() =>
            ruleBuilder.GreaterThan(0).WithMessage(ErrorMessages.VALIDATION_PAGE_INVALID);

        internal IRuleBuilderOptions<TRequest, int> PageSize() =>
            ruleBuilder
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage(ErrorMessages.VALIDATION_PAGE_SIZE_INVALID)
                .LessThanOrEqualTo(MAXIMUM_PAGE_SIZE).WithMessage(ErrorMessages.VALIDATION_PAGE_SIZE_INVALID);
    }
}
