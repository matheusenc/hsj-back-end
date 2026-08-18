using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestCategoryJsonBuilder
{
    public static RequestCategoryJson Build()
    {
        return new Faker<RequestCategoryJson>()
            .RuleFor(request => request.Name, faker => faker.Commerce.Department())
            .RuleFor(request => request.Slug, faker => faker.Lorem.Word().ToLowerInvariant())
            .RuleFor(request => request.DisplayOrder, faker => faker.Random.Int(1, 10))
            .Generate();
    }
}
