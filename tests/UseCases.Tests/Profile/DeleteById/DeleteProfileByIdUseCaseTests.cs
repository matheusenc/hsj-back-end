using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using HospitalSaoJose.Application.UseCases.Profile.DeleteById;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Profile.DeleteById;

public class DeleteProfileByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var profile = ProfileBuilder.Build();

        var useCase = CreateUseCase(profile);

        await useCase.Execute(profile.Id);

        profile.Active.ShouldBeFalse();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenProfileIsSystem()
    {
        var profile = ProfileBuilder.Build(isSystem: true);

        var useCase = CreateUseCase(profile);

        var exception = await useCase.Execute(profile.Id).ShouldThrowAsync<ForbiddenAccessException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Forbidden);
        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_PROFILE_IS_SYSTEM]);
        profile.Active.ShouldBeTrue();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenProfileHasActiveUsers()
    {
        var profile = ProfileBuilder.Build();

        var useCase = CreateUseCase(profile, hasActiveUsers: true);

        var exception = await useCase.Execute(profile.Id).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_PROFILE_HAS_ACTIVE_USERS]);
        profile.Active.ShouldBeTrue();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenProfileDoesNotExist()
    {
        var profile = ProfileBuilder.Build();

        var useCase = CreateUseCase(profile);

        var exception = await useCase.Execute(Guid.CreateVersion7()).ShouldThrowAsync<NotFoundException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        exception.GetErrorMessages().ShouldBe([ErrorMessages.PROFILE_NOT_FOUND]);
    }

    private static DeleteProfileByIdUseCase CreateUseCase(HospitalSaoJose.Domain.Entities.Profile profile, bool hasActiveUsers = false)
    {
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (hasActiveUsers)
            userReadOnlyRepositoryBuilder.ExistActiveUserWithProfile(profile.Id);

        return new DeleteProfileByIdUseCase(
            new IProfileUpdateOnlyRepositoryBuilder().GetById(profile).Build(),
            userReadOnlyRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build());
    }
}
