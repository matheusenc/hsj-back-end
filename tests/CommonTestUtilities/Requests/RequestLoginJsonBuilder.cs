using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(request => request.Email, faker => faker.Internet.Email())
            .RuleFor(request => request.Password, faker => faker.Internet.Password(prefix: "Aa1!"))
            .Generate();
    }
}
