using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using HospitalSaoJose.Application.UseCases.Login.WithEmailAndPassword;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;
        request.Password = password;

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.AccessToken.ShouldBe(IAccessTokenGeneratorBuilder.TOKEN);
        result.ExpiresAtUtc.ShouldBeGreaterThan(DateTime.UtcNow);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenEmailIsNotRegistered()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);
        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_LOGIN_INVALID]);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPasswordIsWrong()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;
        request.Password = "senha-errada";

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_LOGIN_INVALID]);
    }

    private static LoginWithEmailAndPasswordUseCase CreateUseCase(HospitalSaoJose.Domain.Entities.User user)
    {
        var userReadOnlyRepository = new IUserReadOnlyRepositoryBuilder().GetByEmail(user).Build();

        return new LoginWithEmailAndPasswordUseCase(
            userReadOnlyRepository,
            IPasswordHasherBuilder.Build(),
            IAccessTokenGeneratorBuilder.Build());
    }
}
