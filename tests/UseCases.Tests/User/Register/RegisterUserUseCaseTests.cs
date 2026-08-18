using System.Net;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using HospitalSaoJose.Application.UseCases.User.Register;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Register;

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var profile = ProfileBuilder.Build();
        var request = RequestRegisterUserJsonBuilder.Build(profile.Id);

        var useCase = CreateUseCase(profile);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(request.Name);
        result.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Execute_ShouldHashThePassword()
    {
        var profile = ProfileBuilder.Build();
        var request = RequestRegisterUserJsonBuilder.Build(profile.Id);

        var writeRepository = new IUserWriteOnlyRepositoryBuilder();
        var useCase = CreateUseCase(profile, writeRepository);

        await useCase.Execute(request);

        writeRepository.AddedUser.ShouldNotBeNull();
        writeRepository.AddedUser.Password.ShouldBe(PasswordHasherFake.Hash(request.Password));
        writeRepository.AddedUser.Password.ShouldNotBe(request.Password);
        writeRepository.AddedUser.ProfileId.ShouldBe(profile.Id);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var profile = ProfileBuilder.Build();
        var request = RequestRegisterUserJsonBuilder.Build(profile.Id);

        var useCase = CreateUseCase(profile, existingEmail: request.Email);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_EMAIL_ALREADY_EXISTS);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenProfileDoesNotExist()
    {
        var profile = ProfileBuilder.Build();
        var request = RequestRegisterUserJsonBuilder.Build(Guid.CreateVersion7());

        var useCase = CreateUseCase(profile);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.PROFILE_NOT_FOUND);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenEmailIsInvalid()
    {
        var profile = ProfileBuilder.Build();
        var request = RequestRegisterUserJsonBuilder.Build(profile.Id);
        request.Email = "nao-e-um-email";

        var useCase = CreateUseCase(profile);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_EMAIL_INVALID);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPasswordIsTooShort()
    {
        var profile = ProfileBuilder.Build();
        var request = RequestRegisterUserJsonBuilder.Build(profile.Id);
        request.Password = "abc";

        var useCase = CreateUseCase(profile);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PASSWORD_MIN_LENGTH);
    }

    private static RegisterUserUseCase CreateUseCase(
        HospitalSaoJose.Domain.Entities.Profile profile,
        IUserWriteOnlyRepositoryBuilder? writeRepository = null,
        string? existingEmail = null)
    {
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (existingEmail is not null)
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(existingEmail);

        return new RegisterUserUseCase(
            userReadOnlyRepositoryBuilder.Build(),
            (writeRepository ?? new IUserWriteOnlyRepositoryBuilder()).Build(),
            new IProfileReadOnlyRepositoryBuilder().GetById(profile).Build(),
            IPasswordHasherBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
