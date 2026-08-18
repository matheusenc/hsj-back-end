using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterRoleJsonBuilder
{
    public static RequestRegisterRoleJson Build()
    {
        return new Faker<RequestRegisterRoleJson>()
            .RuleFor(request => request.Key, faker => $"{faker.Lorem.Word().ToLowerInvariant()}:{faker.Lorem.Word().ToLowerInvariant()}")
            .RuleFor(request => request.Name, faker => faker.Commerce.Department())
            .RuleFor(request => request.Description, faker => faker.Lorem.Sentence())
            .Generate();
    }
}
