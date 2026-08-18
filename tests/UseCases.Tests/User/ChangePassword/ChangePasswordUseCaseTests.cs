using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using HospitalSaoJose.Application.UseCases.User.ChangePassword;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.ChangePassword;

public class ChangePasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build(password);

        var useCase = CreateUseCase(user);

        await useCase.Execute(request);

        user.Password.ShouldBe(PasswordHasherFake.Hash(request.NewPassword));
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenCurrentPasswordIsWrong()
    {
        var (user, _) = UserBuilder.Build();
        var passwordBefore = user.Password;
        var request = RequestChangePasswordJsonBuilder.Build("senha-errada");

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_CURRENT_PASSWORD_INVALID);
        user.Password.ShouldBe(passwordBefore);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenNewPasswordIsTooShort()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build(password);
        request.NewPassword = "abc";

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PASSWORD_MIN_LENGTH);
    }

    private static ChangePasswordUseCase CreateUseCase(HospitalSaoJose.Domain.Entities.User user)
    {
        return new ChangePasswordUseCase(
            ILoggedUserBuilder.Build(user),
            new IUserUpdateOnlyRepositoryBuilder().GetById(user).Build(),
            IPasswordHasherBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
