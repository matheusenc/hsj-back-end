using System.Net;
using System.Net.Http.Json;
using HospitalSaoJose.Communication.Responses;
using Shouldly;

namespace WebApi.Tests.Category;

public class GetAllCategoriesTests : BaseIntegrationTest
{
    public GetAllCategoriesTests(HospitalSaoJoseApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Success_WithoutToken()
    {
        // O site público lê as categorias sem autenticação, como o transparencia.js fazia no Firestore.
        var response = await Get("categories");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var categories = await response.Content.ReadFromJsonAsync<ResponseCategoriesJson>(JsonOptions);

        categories!.Categories.Select(category => category.Slug)
            .ShouldBe(["convenio", "conveniogov", "emenda", "resdelibport"], ignoreOrder: true);
    }

    [Fact]
    public async Task Error_WhenRegisteringWithoutToken()
    {
        var response = await Post("categories", new { Name = "Nova", Slug = "nova", DisplayOrder = 9 });

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
