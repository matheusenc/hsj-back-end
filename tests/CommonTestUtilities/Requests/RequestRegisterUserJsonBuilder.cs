using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(Guid? profileId = null)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(request => request.Name, faker => faker.Person.FullName)
            .RuleFor(request => request.Email, faker => faker.Internet.Email())
            .RuleFor(request => request.Password, faker => faker.Internet.Password(prefix: "Aa1!"))
            .RuleFor(request => request.ProfileId, _ => profileId ?? Guid.CreateVersion7())
            .Generate();
    }
}
