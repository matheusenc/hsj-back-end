using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Files;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Storage;
using HospitalSaoJose.Application.UseCases.Document.Download;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Document.Download;

public class DownloadDocumentUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var document = DocumentBuilder.Build();

        var useCase = CreateUseCase(document, fileExists: true);

        var result = await useCase.Execute(document.Id);

        result.ShouldNotBeNull();
        result.FileName.ShouldBe(document.OriginalFileName);
        result.ContentType.ShouldBe(document.ContentType);
        result.Content.ShouldNotBeNull();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenDocumentDoesNotExist()
    {
        var document = DocumentBuilder.Build();

        var useCase = CreateUseCase(document, fileExists: true);

        var exception = await useCase.Execute(Guid.CreateVersion7()).ShouldThrowAsync<NotFoundException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        exception.GetErrorMessages().ShouldBe([ErrorMessages.DOCUMENT_NOT_FOUND]);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenFileIsMissingFromStorage()
    {
        var document = DocumentBuilder.Build();

        var useCase = CreateUseCase(document, fileExists: false);

        var exception = await useCase.Execute(document.Id).ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldBe([ErrorMessages.DOCUMENT_FILE_NOT_FOUND]);
    }

    private static DownloadDocumentUseCase CreateUseCase(HospitalSaoJose.Domain.Entities.Document document, bool fileExists)
    {
        var storageBuilder = new IFileStorageServiceBuilder();
        if (fileExists)
            storageBuilder.Get(document.StoredFileName, FileBuilder.Pdf());

        return new DownloadDocumentUseCase(
            new IDocumentReadOnlyRepositoryBuilder().GetById(document).Build(),
            storageBuilder.Build());
    }
}
