using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.Document;
using HospitalSaoJose.Exception;
using Shouldly;

namespace Validators.Tests.Document;

public class DocumentValidatorTests
{
    [Fact]
    public void Success()
    {
        var result = new DocumentValidator().Validate(RequestDocumentJsonBuilder.Build());

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_WhenExternalLinkIsEmpty()
    {
        var request = RequestDocumentJsonBuilder.Build();
        request.ExternalLink = null;

        var result = new DocumentValidator().Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_WhenTitleIsEmpty()
    {
        var request = RequestDocumentJsonBuilder.Build();
        request.Title = string.Empty;

        var result = new DocumentValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_TITLE_REQUIRED);
    }

    [Fact]
    public void Error_WhenTitleExceedsMaxLength()
    {
        var request = RequestDocumentJsonBuilder.Build();
        request.Title = new string('a', 256);

        var result = new DocumentValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_TITLE_MAX_LENGTH);
    }

    [Fact]
    public void Error_WhenDescriptionIsEmpty()
    {
        var request = RequestDocumentJsonBuilder.Build();
        request.Description = string.Empty;

        var result = new DocumentValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_DESCRIPTION_REQUIRED);
    }

    [Fact]
    public void Error_WhenPublicationDateIsMissing()
    {
        var request = RequestDocumentJsonBuilder.Build();
        request.PublicationDate = default;

        var result = new DocumentValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_PUBLICATION_DATE_REQUIRED);
    }

    [Fact]
    public void Error_WhenCategoryIsMissing()
    {
        var request = RequestDocumentJsonBuilder.Build();
        request.CategoryId = Guid.Empty;

        var result = new DocumentValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_CATEGORY_REQUIRED);
    }

    [Theory]
    [InlineData("nao-e-url")]
    [InlineData("www.exemplo.com")]
    public void Error_WhenExternalLinkIsNotAnAbsoluteUrl(string link)
    {
        var request = RequestDocumentJsonBuilder.Build();
        request.ExternalLink = link;

        var result = new DocumentValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_EXTERNAL_LINK_INVALID);
    }
}
