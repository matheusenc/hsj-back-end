using FluentValidation;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Profile;

public class ProfileValidator : AbstractValidator<RequestProfileJson>
{
    public ProfileValidator()
    {
        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_PROFILE_NAME_REQUIRED)
            .MaximumLength(100).WithMessage(ErrorMessages.VALIDATION_PROFILE_NAME_MAX_LENGTH);

        RuleFor(request => request.RoleIds)
            .Must(roleIds => roleIds.Count > 0).WithMessage(ErrorMessages.VALIDATION_PROFILE_AT_LEAST_ONE_ROLE);
    }
}
