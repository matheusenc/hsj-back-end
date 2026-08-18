using FluentValidation;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Role.Register;

public class RegisterRoleValidator : AbstractValidator<RequestRegisterRoleJson>
{
    internal const string KEY_PATTERN = "^[a-z0-9-]+:[a-z0-9-]+$";

    public RegisterRoleValidator()
    {
        RuleFor(request => request.Key)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_ROLE_KEY_REQUIRED)
            .MaximumLength(100).WithMessage(ErrorMessages.VALIDATION_ROLE_KEY_MAX_LENGTH)
            .Matches(KEY_PATTERN).WithMessage(ErrorMessages.VALIDATION_ROLE_KEY_INVALID_FORMAT);

        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_ROLE_NAME_REQUIRED)
            .MaximumLength(150).WithMessage(ErrorMessages.VALIDATION_ROLE_NAME_MAX_LENGTH);
    }
}
