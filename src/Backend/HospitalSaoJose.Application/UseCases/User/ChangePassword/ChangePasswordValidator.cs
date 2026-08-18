using FluentValidation;
using HospitalSaoJose.Application.UseCases.Shared.Validators;
using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(request => request.NewPassword).Password();
    }
}
