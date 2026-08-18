using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace WebApi.Tests;

public abstract class BaseIntegrationTest : IClassFixture<HospitalSaoJoseApplicationFactory>
{
    protected static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;

    protected BaseIntegrationTest(HospitalSaoJoseApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> Post(string requestUri, object request, string accessToken = "")
    {
        AuthorizeWith(accessToken);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> PostFormData(string requestUri, MultipartFormDataContent content, string accessToken = "")
    {
        AuthorizeWith(accessToken);

        return await _httpClient.PostAsync(requestUri, content);
    }

    protected async Task<HttpResponseMessage> Get(string requestUri, string accessToken = "")
    {
        AuthorizeWith(accessToken);

        return await _httpClient.GetAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> Delete(string requestUri, string accessToken = "")
    {
        AuthorizeWith(accessToken);

        return await _httpClient.DeleteAsync(requestUri);
    }

    protected async Task<string> LoginAs(string email, string password)
    {
        var response = await Post("auth/login", new RequestLoginJson { Email = email, Password = password });

        response.EnsureSuccessStatusCode();

        var tokens = await response.Content.ReadFromJsonAsync<ResponseTokensJson>(JsonOptions);

        return tokens!.AccessToken;
    }

    protected Task<string> LoginAsAdmin() =>
        LoginAs(HospitalSaoJoseApplicationFactory.ADMIN_EMAIL, HospitalSaoJoseApplicationFactory.ADMIN_PASSWORD);

    private void AuthorizeWith(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(accessToken)
            ? null
            : new AuthenticationHeaderValue("Bearer", accessToken);
    }
}
