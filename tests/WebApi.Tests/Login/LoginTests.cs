using System.Net;
using System.Net.Http.Json;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Exception;
using Shouldly;

namespace WebApi.Tests.Login;

public class LoginTests : BaseIntegrationTest
{
    public LoginTests(HospitalSaoJoseApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = HospitalSaoJoseApplicationFactory.ADMIN_EMAIL,
            Password = HospitalSaoJoseApplicationFactory.ADMIN_PASSWORD
        };

        var response = await Post("auth/login", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var tokens = await response.Content.ReadFromJsonAsync<ResponseTokensJson>(JsonOptions);
        tokens!.AccessToken.ShouldNotBeNullOrWhiteSpace();
        tokens.ExpiresAtUtc.ShouldBeGreaterThan(DateTime.UtcNow);
    }

    [Fact]
    public async Task Error_WhenPasswordIsWrong()
    {
        var request = new RequestLoginJson
        {
            Email = HospitalSaoJoseApplicationFactory.ADMIN_EMAIL,
            Password = "senha-errada"
        };

        var response = await Post("auth/login", request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var error = await response.Content.ReadFromJsonAsync<ResponseErrorJson>(JsonOptions);
        error!.Errors.ShouldBe([ErrorMessages.VALIDATION_LOGIN_INVALID]);
    }

    [Fact]
    public async Task Error_WhenEmailIsNotRegistered()
    {
        var request = new RequestLoginJson { Email = "ninguem@teste.com.br", Password = "Qualquer@123" };

        var response = await Post("auth/login", request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
