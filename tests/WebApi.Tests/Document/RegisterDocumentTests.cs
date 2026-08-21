using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommonTestUtilities.Files;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Exception;
using Shouldly;

namespace WebApi.Tests.Document;

public class RegisterDocumentTests : BaseIntegrationTest
{
    public RegisterDocumentTests(HospitalSaoJoseApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Success_AndTheDocumentBecomesPubliclyVisible()
    {
        var token = await LoginAsAdmin();
        var categoryId = await FirstCategoryId("convenio");

        var title = $"Contrato {Guid.CreateVersion7()}";
        var response = await PostFormData("documents", FormDataFor(categoryId, title), token);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<ResponseRegisteredDocumentJson>(JsonOptions);
        created!.Title.ShouldBe(title);
        created.DownloadUrl.ShouldBe($"/documents/{created.Id}/download");

        // Sem token, como o site público faz.
        var listResponse = await Get("documents?categorySlug=convenio&page=1&pageSize=9");
        listResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var list = await listResponse.Content.ReadFromJsonAsync<ResponseDocumentsJson>(JsonOptions);
        list!.Documents.ShouldContain(document => document.Id == created.Id);

        var downloadResponse = await Get($"documents/{created.Id}/download");
        downloadResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        downloadResponse.Content.Headers.ContentType!.MediaType.ShouldBe("application/pdf");
        (await downloadResponse.Content.ReadAsByteArrayAsync()).Length.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Error_WithoutToken()
    {
        var categoryId = await FirstCategoryId("convenio");

        var response = await PostFormData("documents", FormDataFor(categoryId, "Sem token"));

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_WhenFileIsNotAPdf()
    {
        var token = await LoginAsAdmin();
        var categoryId = await FirstCategoryId("convenio");

        var content = FormDataFor(categoryId, "Arquivo invalido", pdf: false);

        var response = await PostFormData("documents", content, token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ResponseErrorJson>(JsonOptions);
        error!.Errors.ShouldContain(ErrorMessages.VALIDATION_FILE_CONTENT_MISMATCH);
    }

    [Fact]
    public async Task Error_WhenFileIsMissing()
    {
        var token = await LoginAsAdmin();
        var categoryId = await FirstCategoryId("convenio");

        var content = new MultipartFormDataContent
        {
            { new StringContent(categoryId.ToString()), "CategoryId" },
            { new StringContent("Sem arquivo"), "Title" },
            { new StringContent("Descrição"), "Description" },
            { new StringContent(DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)), "PublicationDate" }
        };

        var response = await PostFormData("documents", content, token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ResponseErrorJson>(JsonOptions);
        error!.Errors.ShouldContain(ErrorMessages.VALIDATION_FILE_REQUIRED);
    }

    private static MultipartFormDataContent FormDataFor(Guid categoryId, string title, bool pdf = true)
    {
        var file = pdf ? FileBuilder.Pdf() : FileBuilder.NotAPdf();

        var fileContent = new StreamContent(file);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        return new MultipartFormDataContent
        {
            { new StringContent(categoryId.ToString()), "CategoryId" },
            { new StringContent(title), "Title" },
            { new StringContent("Documento publicado pelo teste de integração."), "Description" },
            { new StringContent(DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)), "PublicationDate" },
            { fileContent, "file", "documento.pdf" }
        };
    }

    private async Task<Guid> FirstCategoryId(string slug)
    {
        var response = await Get("categories");
        var categories = await response.Content.ReadFromJsonAsync<ResponseCategoriesJson>(JsonOptions);

        return categories!.Categories.First(category => category.Slug == slug).Id;
    }
}
