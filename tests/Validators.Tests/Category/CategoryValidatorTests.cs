using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.Category;
using HospitalSaoJose.Exception;
using Shouldly;

namespace Validators.Tests.Category;

public class CategoryValidatorTests
{
    [Fact]
    public void Success()
    {
        var result = new CategoryValidator().Validate(RequestCategoryJsonBuilder.Build());

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("convenio")]
    [InlineData("conveniogov")]
    [InlineData("resdelibport")]
    [InlineData("emendas-parlamentares")]
    public void Success_WithTheLegacySlugs(string slug)
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Slug = slug;

        var result = new CategoryValidator().Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_WhenNameIsEmpty()
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Name = string.Empty;

        var result = new CategoryValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_CATEGORY_NAME_REQUIRED);
    }

    [Theory]
    [InlineData("Convenio")]
    [InlineData("convenio municipal")]
    [InlineData("convênio")]
    [InlineData("convenio_gov")]
    public void Error_WhenSlugFormatIsInvalid(string slug)
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Slug = slug;

        var result = new CategoryValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_CATEGORY_SLUG_INVALID_FORMAT);
    }

    [Fact]
    public void Error_WhenSlugIsEmpty()
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Slug = string.Empty;

        var result = new CategoryValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_CATEGORY_SLUG_REQUIRED);
    }
}
