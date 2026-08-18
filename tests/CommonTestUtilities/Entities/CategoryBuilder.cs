using Bogus;
using HospitalSaoJose.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class CategoryBuilder
{
    public static Category Build(string? slug = null)
    {
        return new Faker<Category>()
            .RuleFor(category => category.Name, faker => faker.Commerce.Department())
            .RuleFor(category => category.Slug, faker => slug ?? faker.Lorem.Word().ToLowerInvariant())
            .RuleFor(category => category.DisplayOrder, faker => faker.Random.Int(1, 10))
            .Generate();
    }
}
