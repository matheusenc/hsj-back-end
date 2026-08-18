using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build(Guid? profileId = null)
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, faker => faker.Person.FullName)
            .RuleFor(request => request.Email, faker => faker.Internet.Email())
            .RuleFor(request => request.ProfileId, _ => profileId ?? Guid.CreateVersion7())
            .Generate();
    }
}
