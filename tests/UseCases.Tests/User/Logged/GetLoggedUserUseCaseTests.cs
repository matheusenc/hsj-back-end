using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using HospitalSaoJose.Application.UseCases.User.Logged;
using Shouldly;

namespace UseCases.Tests.User.Logged;

public class GetLoggedUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var roles = RoleBuilder.Collection();
        var profile = ProfileBuilder.Build(roles: roles);
        var (user, _) = UserBuilder.Build(profile);

        var useCase = new GetLoggedUserUseCase(ILoggedUserBuilder.Build(user));

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
        result.Email.ShouldBe(user.Email);
        result.Profile.Id.ShouldBe(profile.Id);
        result.Profile.Name.ShouldBe(profile.Name);
        result.Permissions.ShouldBe(roles.Select(role => role.Key), ignoreOrder: true);
    }

    [Fact]
    public async Task Execute_ShouldIgnoreInactiveRoles()
    {
        var roles = RoleBuilder.Collection();
        roles[0].Active = false;

        var profile = ProfileBuilder.Build(roles: roles);
        var (user, _) = UserBuilder.Build(profile);

        var useCase = new GetLoggedUserUseCase(ILoggedUserBuilder.Build(user));

        var result = await useCase.Execute();

        result.Permissions.Count.ShouldBe(roles.Count - 1);
        result.Permissions.ShouldNotContain(roles[0].Key);
    }
}
