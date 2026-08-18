using Bogus;
using HospitalSaoJose.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class RoleBuilder
{
    public static Role Build(string? key = null, bool isSystem = false)
    {
        return new Faker<Role>()
            .RuleFor(role => role.Key, faker => key ?? $"{faker.Lorem.Word()}:{faker.Lorem.Word()}")
            .RuleFor(role => role.Name, faker => faker.Commerce.Department())
            .RuleFor(role => role.Description, faker => faker.Lorem.Sentence())
            .RuleFor(role => role.IsSystem, _ => isSystem)
            .Generate();
    }

    public static List<Role> Collection(int count = 3)
    {
        return Enumerable.Range(0, count).Select(index => Build($"recurso{index}:acao")).ToList();
    }
}
