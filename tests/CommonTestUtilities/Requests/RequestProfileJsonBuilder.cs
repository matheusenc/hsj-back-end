using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestProfileJsonBuilder
{
    public static RequestProfileJson Build(List<Guid>? roleIds = null)
    {
        return new Faker<RequestProfileJson>()
            .RuleFor(request => request.Name, faker => faker.Commerce.Department())
            .RuleFor(request => request.Description, faker => faker.Lorem.Sentence())
            .RuleFor(request => request.RoleIds, _ => roleIds ?? [Guid.CreateVersion7()])
            .Generate();
    }
}
