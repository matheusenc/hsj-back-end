using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Files;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Storage;
using HospitalSaoJose.Application.UseCases.Document;
using HospitalSaoJose.Application.UseCases.Document.Register;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Document.Register;

public class RegisterDocumentUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        var file = PdfFile();

        var writeRepository = new IDocumentWriteOnlyRepositoryBuilder();
        var useCase = CreateUseCase(category, writeRepository);

        var result = await useCase.Execute(request, file);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);
        result.DownloadUrl.ShouldBe($"/documents/{result.Id}/download");

        writeRepository.AddedDocument.ShouldNotBeNull();
        writeRepository.AddedDocument.StoredFileName.ShouldBe(IFileStorageServiceBuilder.STORED_FILE_NAME);
        writeRepository.AddedDocument.OriginalFileName.ShouldBe(file.FileName);
        writeRepository.AddedDocument.ContentType.ShouldBe("application/pdf");
        writeRepository.AddedDocument.CategoryId.ShouldBe(category.Id);
        writeRepository.AddedDocument.CreatedByUserId.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenFileIsMissing()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, file: null).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_FILE_REQUIRED);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenContentDoesNotMatchExtension()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        var content = FileBuilder.NotAPdf();
        var file = new DocumentFile(content, "documento.pdf", content.Length);

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, file).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_FILE_CONTENT_MISMATCH);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenExtensionIsNotAccepted()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        var content = FileBuilder.Pdf();
        var file = new DocumentFile(content, "instalador.exe", content.Length);

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, file).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_FILE_TYPE_NOT_ACCEPTED);
    }

    [Fact]
    public async Task Execute_ShouldAcceptSpreadsheet_AndKeepItsOwnExtension()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        var content = FileBuilder.Xlsx();
        var file = new DocumentFile(content, "Execucao orcamentaria.XLSX", content.Length);

        var useCase = CreateUseCase(category);

        var response = await useCase.Execute(request, file);

        response.ShouldNotBeNull();
    }

    [Fact]
    public async Task Execute_ShouldStripDangerousMarkupFromDescription()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        request.Description =
            """
            <p>Veja o <a href="https://exemplo.org/edital">edital</a>.</p>
            <script>alert('xss')</script>
            <img src=x onerror="alert(1)">
            <a href="javascript:alert(1)">clique aqui</a>
            <iframe src="https://exemplo.org"></iframe>
            """;

        var useCase = CreateUseCase(category);

        await useCase.Execute(request, PdfFile());

        // O use case higieniza o request antes de gravar, então o que sobrou
        // aqui é exatamente o que foi para o banco.
        request.Description.ShouldNotContain("<script");
        request.Description.ShouldNotContain("onerror");
        request.Description.ShouldNotContain("javascript:");
        request.Description.ShouldNotContain("<iframe");
        request.Description.ShouldNotContain("<img");

        request.Description.ShouldContain("https://exemplo.org/edital");
        request.Description.ShouldContain("rel=\"noopener noreferrer\"");
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenDescriptionHasOnlyMarkup()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        request.Description = "<p></p><br>";

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, PdfFile()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_DESCRIPTION_REQUIRED);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenFileIsTooLarge()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        var content = FileBuilder.Pdf();
        var file = new DocumentFile(content, "documento.pdf", DocumentFileValidatorLimit + 1);

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, file).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_FILE_MAX_SIZE);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(Guid.CreateVersion7());

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, PdfFile()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.CATEGORY_NOT_FOUND);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenTitleIsEmpty()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        request.Title = string.Empty;

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, PdfFile()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_TITLE_REQUIRED);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPublicationDateIsMissing()
    {
        var category = CategoryBuilder.Build();
        var request = RequestDocumentJsonBuilder.Build(category.Id);
        request.PublicationDate = default;

        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(request, PdfFile()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PUBLICATION_DATE_REQUIRED);
    }

    private const long DocumentFileValidatorLimit = 25 * 1024 * 1024;

    private static DocumentFile PdfFile()
    {
        var content = FileBuilder.Pdf();

        return new DocumentFile(content, "documento.pdf", content.Length);
    }

    private static RegisterDocumentUseCase CreateUseCase(
        HospitalSaoJose.Domain.Entities.Category category,
        IDocumentWriteOnlyRepositoryBuilder? writeRepository = null)
    {
        var (user, _) = UserBuilder.Build();

        return new RegisterDocumentUseCase(
            (writeRepository ?? new IDocumentWriteOnlyRepositoryBuilder()).Build(),
            new ICategoryReadOnlyRepositoryBuilder().GetById(category).Build(),
            new IFileStorageServiceBuilder().Build(),
            ILoggedUserBuilder.Build(user),
            IUnitOfWorkBuilder.Build());
    }
}
