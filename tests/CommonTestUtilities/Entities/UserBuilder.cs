using Bogus;
using CommonTestUtilities.Cryptography;
using HospitalSaoJose.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static (User User, string Password) Build(Profile? profile = null)
    {
        var password = "Senha@123";
        var userProfile = profile ?? ProfileBuilder.Build(roles: RoleBuilder.Collection());

        var user = new Faker<User>()
            .RuleFor(item => item.Name, faker => faker.Person.FullName)
            .RuleFor(item => item.Email, faker => faker.Internet.Email())
            .RuleFor(item => item.Password, _ => PasswordHasherFake.Hash(password))
            .RuleFor(item => item.ProfileId, _ => userProfile.Id)
            .RuleFor(item => item.Profile, _ => userProfile)
            .Generate();

        return (user, password);
    }
}
