using FluentValidation;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.User.UpdateById;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_NAME_REQUIRED)
            .MaximumLength(255).WithMessage(ErrorMessages.VALIDATION_NAME_MAX_LENGTH);

        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_EMAIL_REQUIRED)
            .EmailAddress().WithMessage(ErrorMessages.VALIDATION_EMAIL_INVALID);

        RuleFor(request => request.ProfileId)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.VALIDATION_PROFILE_REQUIRED);
    }
}
