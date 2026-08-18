using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using HospitalSaoJose.Application.UseCases.Role.DeleteById;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Role.DeleteById;

public class DeleteRoleByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var role = RoleBuilder.Build();

        var useCase = CreateUseCase(role);

        await useCase.Execute(role.Id);

        role.Active.ShouldBeFalse();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenRoleIsSystem()
    {
        var role = RoleBuilder.Build(isSystem: true);

        var useCase = CreateUseCase(role);

        var exception = await useCase.Execute(role.Id).ShouldThrowAsync<ForbiddenAccessException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Forbidden);
        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_ROLE_IS_SYSTEM]);
        role.Active.ShouldBeTrue();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenRoleIsLinkedToAProfile()
    {
        var role = RoleBuilder.Build();

        var useCase = CreateUseCase(role, isInUse: true);

        var exception = await useCase.Execute(role.Id).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldBe([ErrorMessages.VALIDATION_ROLE_IN_USE]);
        role.Active.ShouldBeTrue();
    }

    private static DeleteRoleByIdUseCase CreateUseCase(HospitalSaoJose.Domain.Entities.Role role, bool isInUse = false)
    {
        var roleReadOnlyRepositoryBuilder = new IRoleReadOnlyRepositoryBuilder();
        if (isInUse)
            roleReadOnlyRepositoryBuilder.ExistProfileUsingRole(role.Id);

        return new DeleteRoleByIdUseCase(
            roleReadOnlyRepositoryBuilder.Build(),
            new IRoleUpdateOnlyRepositoryBuilder().GetById(role).Build(),
            IUnitOfWorkBuilder.Build());
    }
}
