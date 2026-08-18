using FluentValidation;
using HospitalSaoJose.Application.UseCases.Shared.Validators;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_NAME_REQUIRED)
            .MaximumLength(255).WithMessage(ErrorMessages.VALIDATION_NAME_MAX_LENGTH);

        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_EMAIL_REQUIRED)
            .EmailAddress().WithMessage(ErrorMessages.VALIDATION_EMAIL_INVALID);

        RuleFor(request => request.Password).Password();

        RuleFor(request => request.ProfileId)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.VALIDATION_PROFILE_REQUIRED);
    }
}
