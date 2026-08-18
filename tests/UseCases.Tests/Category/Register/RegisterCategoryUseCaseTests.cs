using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.Category.Register;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Category.Register;

public class RegisterCategoryUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCategoryJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Slug.ShouldBe(request.Slug);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenSlugAlreadyExists()
    {
        var request = RequestCategoryJsonBuilder.Build();

        var useCase = CreateUseCase(existingSlug: request.Slug);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_CATEGORY_SLUG_ALREADY_EXISTS);
    }

    [Theory]
    [InlineData("Convenio")]
    [InlineData("convenio municipal")]
    [InlineData("convênio")]
    public async Task Execute_ShouldThrowException_WhenSlugFormatIsInvalid(string slug)
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Slug = slug;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_CATEGORY_SLUG_INVALID_FORMAT);
    }

    private static RegisterCategoryUseCase CreateUseCase(string? existingSlug = null)
    {
        var categoryReadOnlyRepositoryBuilder = new ICategoryReadOnlyRepositoryBuilder();
        if (existingSlug is not null)
            categoryReadOnlyRepositoryBuilder.ExistActiveCategoryWithSlug(existingSlug);

        return new RegisterCategoryUseCase(
            categoryReadOnlyRepositoryBuilder.Build(),
            ICategoryWriteOnlyRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
