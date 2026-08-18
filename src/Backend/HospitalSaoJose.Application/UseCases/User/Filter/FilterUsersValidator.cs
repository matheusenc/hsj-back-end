using FluentValidation;
using HospitalSaoJose.Application.UseCases.Shared.Validators;
using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.User.Filter;

public class FilterUsersValidator : AbstractValidator<RequestFilterUsersJson>
{
    public FilterUsersValidator()
    {
        RuleFor(request => request.Page).Page();
        RuleFor(request => request.PageSize).PageSize();
    }
}
