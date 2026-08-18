using FluentValidation;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Role.UpdateById;

public class UpdateRoleValidator : AbstractValidator<RequestUpdateRoleJson>
{
    public UpdateRoleValidator()
    {
        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_ROLE_NAME_REQUIRED)
            .MaximumLength(150).WithMessage(ErrorMessages.VALIDATION_ROLE_NAME_MAX_LENGTH);
    }
}
