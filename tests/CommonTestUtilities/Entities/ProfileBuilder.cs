using Bogus;
using HospitalSaoJose.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class ProfileBuilder
{
    public static Profile Build(bool isSystem = false, List<Role>? roles = null)
    {
        var profile = new Faker<Profile>()
            .RuleFor(item => item.Name, faker => faker.Commerce.Department())
            .RuleFor(item => item.Description, faker => faker.Lorem.Sentence())
            .RuleFor(item => item.IsSystem, _ => isSystem)
            .Generate();

        foreach (var role in roles ?? [])
            profile.ProfileRoles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = role.Id, Role = role, Profile = profile });

        return profile;
    }
}
