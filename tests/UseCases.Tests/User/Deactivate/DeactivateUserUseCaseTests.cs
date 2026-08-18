using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using HospitalSaoJose.Application.UseCases.User.Deactivate;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Deactivate;

public class DeactivateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (loggedUser, _) = UserBuilder.Build();
        var (target, _) = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, target);

        await useCase.Execute(target.Id);

        target.Active.ShouldBeFalse();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenUserTriesToDeactivateItself()
    {
        var (loggedUser, _) = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, loggedUser);

        var exception = await useCase.Execute(loggedUser.Id).ShouldThrowAsync<ForbiddenAccessException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Forbidden);
        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_ACCESS_DENIED]);
        loggedUser.Active.ShouldBeTrue();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenUserDoesNotExist()
    {
        var (loggedUser, _) = UserBuilder.Build();
        var (target, _) = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, target);

        var exception = await useCase.Execute(Guid.CreateVersion7()).ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldBe([ErrorMessages.USER_NOT_FOUND]);
    }

    private static DeactivateUserUseCase CreateUseCase(
        HospitalSaoJose.Domain.Entities.User loggedUser,
        HospitalSaoJose.Domain.Entities.User target)
    {
        return new DeactivateUserUseCase(
            ILoggedUserBuilder.Build(loggedUser),
            new IUserUpdateOnlyRepositoryBuilder().GetById(target).Build(),
            IUnitOfWorkBuilder.Build());
    }
}
