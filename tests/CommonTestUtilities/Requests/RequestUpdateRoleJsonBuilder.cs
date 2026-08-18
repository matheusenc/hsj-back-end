using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateRoleJsonBuilder
{
    public static RequestUpdateRoleJson Build()
    {
        return new Faker<RequestUpdateRoleJson>()
            .RuleFor(request => request.Name, faker => faker.Commerce.Department())
            .RuleFor(request => request.Description, faker => faker.Lorem.Sentence())
            .Generate();
    }
}
