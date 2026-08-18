using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(string? currentPassword = null)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(request => request.CurrentPassword, faker => currentPassword ?? faker.Internet.Password(prefix: "Aa1!"))
            .RuleFor(request => request.NewPassword, faker => faker.Internet.Password(prefix: "Bb2@"))
            .Generate();
    }
}
