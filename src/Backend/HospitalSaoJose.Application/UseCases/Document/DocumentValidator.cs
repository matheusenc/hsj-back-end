using FluentValidation;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Extensions;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Document;

public class DocumentValidator : AbstractValidator<RequestDocumentJson>
{
    public DocumentValidator()
    {
        RuleFor(request => request.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_TITLE_REQUIRED)
            .MaximumLength(255).WithMessage(ErrorMessages.VALIDATION_TITLE_MAX_LENGTH);

        RuleFor(request => request.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_DESCRIPTION_REQUIRED)
            .MaximumLength(2000).WithMessage(ErrorMessages.VALIDATION_DESCRIPTION_MAX_LENGTH);

        RuleFor(request => request.PublicationDate)
            .NotEqual(default(DateOnly)).WithMessage(ErrorMessages.VALIDATION_PUBLICATION_DATE_REQUIRED);

        RuleFor(request => request.CategoryId)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.VALIDATION_CATEGORY_REQUIRED);

        When(request => request.ExternalLink.IsNotEmpty(), () =>
        {
            RuleFor(request => request.ExternalLink)
                .Must(link => Uri.TryCreate(link, UriKind.Absolute, out _))
                .WithMessage(ErrorMessages.VALIDATION_EXTERNAL_LINK_INVALID);
        });
    }
}
