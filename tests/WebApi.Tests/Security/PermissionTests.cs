using System.Net;
using System.Net.Http.Json;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using Shouldly;

namespace WebApi.Tests.Security;

public class PermissionTests : BaseIntegrationTest
{
    private const string EDITOR_PASSWORD = "Editor@12345";

    public PermissionTests(HospitalSaoJoseApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Admin_ShouldSeeEveryRoleAndProfile()
    {
        var token = await LoginAsAdmin();

        var rolesResponse = await Get("roles", token);
        rolesResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var roles = await rolesResponse.Content.ReadFromJsonAsync<ResponseRolesJson>(JsonOptions);
        roles!.Roles.ShouldAllBe(role => role.IsSystem);
        roles.Roles.ShouldContain(role => role.Key == "documents:create");

        // A role de publicar documentos precisa aparecer vinculada ao perfil Editor.
        roles.Roles.First(role => role.Key == "documents:create").Profiles
            .ShouldContain(profile => profile.Name == "Editor");

        var profilesResponse = await Get("profiles", token);
        var profiles = await profilesResponse.Content.ReadFromJsonAsync<ResponseProfilesJson>(JsonOptions);

        profiles!.Profiles.Select(profile => profile.Name).ShouldBe(["Administrador", "Editor"], ignoreOrder: true);
    }

    [Fact]
    public async Task Editor_ShouldPublishDocumentsButNotManageCategories()
    {
        var adminToken = await LoginAsAdmin();
        var editorToken = await CreateEditorAndLogin(adminToken);

        var loggedResponse = await Get("users/me", editorToken);
        var logged = await loggedResponse.Content.ReadFromJsonAsync<ResponseLoggedUserJson>(JsonOptions);

        logged!.Profile.Name.ShouldBe("Editor");
        logged.Permissions.ShouldBe(["documents:create", "documents:update", "documents:delete"], ignoreOrder: true);

        var categoryResponse = await Post("categories", new RequestCategoryJson { Name = "Nova", Slug = "nova", DisplayOrder = 9 }, editorToken);
        categoryResponse.StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        var usersResponse = await Get("users", editorToken);
        usersResponse.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_WhenDeletingASystemProfile()
    {
        var token = await LoginAsAdmin();

        var profilesResponse = await Get("profiles", token);
        var profiles = await profilesResponse.Content.ReadFromJsonAsync<ResponseProfilesJson>(JsonOptions);
        var administrator = profiles!.Profiles.First(profile => profile.Name == "Administrador");

        administrator.IsSystem.ShouldBeTrue();

        var response = await Delete($"profiles/{administrator.Id}", token);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    private async Task<string> CreateEditorAndLogin(string adminToken)
    {
        var profilesResponse = await Get("profiles", adminToken);
        var profiles = await profilesResponse.Content.ReadFromJsonAsync<ResponseProfilesJson>(JsonOptions);
        var editorProfile = profiles!.Profiles.First(profile => profile.Name == "Editor");

        var email = $"editor-{Guid.CreateVersion7()}@teste.com.br";

        var request = new RequestRegisterUserJson
        {
            Name = "Editor de Teste",
            Email = email,
            Password = EDITOR_PASSWORD,
            ProfileId = editorProfile.Id
        };

        var createResponse = await Post("users", request, adminToken);
        createResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

        return await LoginAs(email, EDITOR_PASSWORD);
    }
}
