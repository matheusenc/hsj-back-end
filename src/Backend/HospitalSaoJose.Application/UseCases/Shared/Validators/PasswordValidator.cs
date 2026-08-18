using FluentValidation;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Shared.Validators;

public static class PasswordValidator
{
    private const int MINIMUM_LENGTH = 8;

    extension<TRequest>(IRuleBuilderInitial<TRequest, string> ruleBuilder)
    {
        internal IRuleBuilderOptions<TRequest, string> Password()
        {
            return ruleBuilder
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ErrorMessages.VALIDATION_PASSWORD_REQUIRED)
                .MinimumLength(MINIMUM_LENGTH).WithMessage(ErrorMessages.VALIDATION_PASSWORD_MIN_LENGTH);
        }
    }
}
