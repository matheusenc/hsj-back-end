using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.Profile.Register;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Profile.Register;

public class RegisterProfileUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var roles = RoleBuilder.Collection();
        var request = RequestProfileJsonBuilder.Build(roles.Select(role => role.Id).ToList());

        var writeRepository = new IProfileWriteOnlyRepositoryBuilder();
        var useCase = CreateUseCase(roles, writeRepository);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);

        writeRepository.AddedProfile.ShouldNotBeNull();
        writeRepository.AddedProfile.IsSystem.ShouldBeFalse();
        writeRepository.AddedProfile.ProfileRoles.Count.ShouldBe(roles.Count);
    }

    [Fact]
    public async Task Execute_ShouldNotDuplicateRepeatedRoleIds()
    {
        var roles = RoleBuilder.Collection(2);
        var repeated = new List<Guid> { roles[0].Id, roles[0].Id, roles[1].Id };
        var request = RequestProfileJsonBuilder.Build(repeated);

        var writeRepository = new IProfileWriteOnlyRepositoryBuilder();
        var useCase = CreateUseCase(roles, writeRepository);

        await useCase.Execute(request);

        writeRepository.AddedProfile.ShouldNotBeNull();
        writeRepository.AddedProfile.ProfileRoles.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenNameAlreadyExists()
    {
        var roles = RoleBuilder.Collection();
        var request = RequestProfileJsonBuilder.Build(roles.Select(role => role.Id).ToList());

        var useCase = CreateUseCase(roles, existingName: request.Name);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PROFILE_NAME_ALREADY_EXISTS);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenThereIsNoRole()
    {
        var request = RequestProfileJsonBuilder.Build([]);

        var useCase = CreateUseCase([]);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PROFILE_AT_LEAST_ONE_ROLE);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenSomeRoleDoesNotExist()
    {
        var roles = RoleBuilder.Collection(2);
        var request = RequestProfileJsonBuilder.Build([roles[0].Id, roles[1].Id, Guid.CreateVersion7()]);

        // O repositório devolve só as duas roles conhecidas: a terceira não existe.
        var useCase = CreateUseCase(roles);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldContain(ErrorMessages.VALIDATION_PROFILE_ROLE_NOT_FOUND);
    }

    private static RegisterProfileUseCase CreateUseCase(
        List<HospitalSaoJose.Domain.Entities.Role> roles,
        IProfileWriteOnlyRepositoryBuilder? writeRepository = null,
        string? existingName = null)
    {
        var profileReadOnlyRepositoryBuilder = new IProfileReadOnlyRepositoryBuilder();
        if (existingName is not null)
            profileReadOnlyRepositoryBuilder.ExistActiveProfileWithName(existingName);

        return new RegisterProfileUseCase(
            profileReadOnlyRepositoryBuilder.Build(),
            (writeRepository ?? new IProfileWriteOnlyRepositoryBuilder()).Build(),
            new IRoleReadOnlyRepositoryBuilder().GetByIds(roles).Build(),
            IUnitOfWorkBuilder.Build());
    }
}
