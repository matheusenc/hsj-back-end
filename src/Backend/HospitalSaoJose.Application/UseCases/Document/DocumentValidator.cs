using FluentValidation;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Extensions;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Document;

public class DocumentValidator : AbstractValidator<RequestDocumentJson>
{
    /// <summary>
    /// Acompanha o tamanho da coluna Description. É bem maior que os 2000 de
    /// antes porque agora conta marcação: um link sozinho gasta uns 60
    /// caracteres só de tag.
    /// </summary>
    internal const int MAXIMUM_DESCRIPTION_LENGTH = 8000;

    public DocumentValidator()
    {
        RuleFor(request => request.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorMessages.VALIDATION_TITLE_REQUIRED)
            .MaximumLength(255).WithMessage(ErrorMessages.VALIDATION_TITLE_MAX_LENGTH);

        // A descrição é HTML: o use case já a higienizou antes de chegar aqui.
        // "Vazio" se mede pelo texto sem marcação — um `<p></p>` não é conteúdo —
        // e o limite se mede no HTML, porque é ele que precisa caber na coluna.
        RuleFor(request => request.Description)
            .Cascade(CascadeMode.Stop)
            .Must(descricao => DocumentDescriptionSanitizer.ToPlainText(descricao ?? string.Empty).IsNotEmpty())
                .WithMessage(ErrorMessages.VALIDATION_DESCRIPTION_REQUIRED)
            .MaximumLength(MAXIMUM_DESCRIPTION_LENGTH).WithMessage(ErrorMessages.VALIDATION_DESCRIPTION_MAX_LENGTH);

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
