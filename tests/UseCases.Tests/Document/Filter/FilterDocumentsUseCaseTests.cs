using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using HospitalSaoJose.Application.UseCases.Document.Filter;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Document.Filter;

public class FilterDocumentsUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var category = CategoryBuilder.Build("convenio");
        var documents = Enumerable.Range(0, 9).Select(_ => DocumentBuilder.Build(category)).ToList();

        var useCase = CreateUseCase(documents, totalCount: 20);

        var result = await useCase.Execute(new RequestFilterDocumentsJson { CategorySlug = "convenio", Page = 1, PageSize = 9 });

        result.ShouldNotBeNull();
        result.Documents.Count.ShouldBe(9);
        result.TotalCount.ShouldBe(20);
        result.TotalPages.ShouldBe(3);
        result.Documents[0].DownloadUrl.ShouldBe($"/documents/{documents[0].Id}/download");
        result.Documents[0].FileName.ShouldBe(documents[0].OriginalFileName);
        result.Documents[0].Category.Slug.ShouldBe("convenio");
    }

    [Fact]
    public async Task Execute_ShouldReturnZeroPages_WhenThereIsNoDocument()
    {
        var useCase = CreateUseCase([], totalCount: 0);

        var result = await useCase.Execute(new RequestFilterDocumentsJson { Page = 1, PageSize = 9 });

        result.Documents.ShouldBeEmpty();
        result.TotalPages.ShouldBe(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Execute_ShouldThrowException_WhenPageIsInvalid(int page)
    {
        var useCase = CreateUseCase([], totalCount: 0);

        var exception = await useCase
            .Execute(new RequestFilterDocumentsJson { Page = page, PageSize = 9 })
            .ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PAGE_INVALID);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPageSizeIsAboveTheLimit()
    {
        var useCase = CreateUseCase([], totalCount: 0);

        var exception = await useCase
            .Execute(new RequestFilterDocumentsJson { Page = 1, PageSize = 101 })
            .ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PAGE_SIZE_INVALID);
    }

    private static FilterDocumentsUseCase CreateUseCase(List<HospitalSaoJose.Domain.Entities.Document> documents, int totalCount)
    {
        return new FilterDocumentsUseCase(new IDocumentReadOnlyRepositoryBuilder().Filter(documents, totalCount).Build());
    }
}
