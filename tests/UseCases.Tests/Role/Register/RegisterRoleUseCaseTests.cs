using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.Role.Register;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Role.Register;

public class RegisterRoleUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterRoleJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Key.ShouldBe(request.Key);
        result.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenKeyAlreadyExists()
    {
        var request = RequestRegisterRoleJsonBuilder.Build();

        var useCase = CreateUseCase(existingKey: request.Key);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_ROLE_KEY_ALREADY_EXISTS);
    }

    [Theory]
    [InlineData("SemDoisPontos")]
    [InlineData("Recurso:Acao")]
    [InlineData("recurso acao")]
    [InlineData("recurso:")]
    public async Task Execute_ShouldThrowException_WhenKeyFormatIsInvalid(string key)
    {
        var request = RequestRegisterRoleJsonBuilder.Build();
        request.Key = key;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_ROLE_KEY_INVALID_FORMAT);
    }

    private static RegisterRoleUseCase CreateUseCase(string? existingKey = null)
    {
        var roleReadOnlyRepositoryBuilder = new IRoleReadOnlyRepositoryBuilder();
        if (existingKey is not null)
            roleReadOnlyRepositoryBuilder.ExistActiveRoleWithKey(existingKey);

        return new RegisterRoleUseCase(
            roleReadOnlyRepositoryBuilder.Build(),
            IRoleWriteOnlyRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
