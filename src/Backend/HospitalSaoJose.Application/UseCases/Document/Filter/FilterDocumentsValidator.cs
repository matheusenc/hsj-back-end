using FluentValidation;
using HospitalSaoJose.Application.UseCases.Shared.Validators;
using HospitalSaoJose.Communication.Requests;

namespace HospitalSaoJose.Application.UseCases.Document.Filter;

public class FilterDocumentsValidator : AbstractValidator<RequestFilterDocumentsJson>
{
    public FilterDocumentsValidator()
    {
        RuleFor(request => request.Page).Page();
        RuleFor(request => request.PageSize).PageSize();
    }
}
