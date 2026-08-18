using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using HospitalSaoJose.Application.UseCases.Category.DeleteById;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Category.DeleteById;

public class DeleteCategoryByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var category = CategoryBuilder.Build();

        var useCase = CreateUseCase(category);

        await useCase.Execute(category.Id);

        category.Active.ShouldBeFalse();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenCategoryHasActiveDocuments()
    {
        var category = CategoryBuilder.Build();

        var useCase = CreateUseCase(category, hasActiveDocuments: true);

        var exception = await useCase.Execute(category.Id).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_CATEGORY_HAS_ACTIVE_DOCUMENTS]);
        category.Active.ShouldBeTrue();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        var category = CategoryBuilder.Build();

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(Guid.CreateVersion7()).ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldBe([ErrorMessages.CATEGORY_NOT_FOUND]);
    }

    private static DeleteCategoryByIdUseCase CreateUseCase(HospitalSaoJose.Domain.Entities.Category category, bool hasActiveDocuments = false)
    {
        var documentReadOnlyRepositoryBuilder = new IDocumentReadOnlyRepositoryBuilder();
        if (hasActiveDocuments)
            documentReadOnlyRepositoryBuilder.ExistActiveDocumentInCategory(category.Id);

        return new DeleteCategoryByIdUseCase(
            new ICategoryUpdateOnlyRepositoryBuilder().GetById(category).Build(),
            documentReadOnlyRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
