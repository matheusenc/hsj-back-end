using Bogus;
using HospitalSaoJose.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestDocumentJsonBuilder
{
    public static RequestDocumentJson Build(Guid? categoryId = null)
    {
        return new Faker<RequestDocumentJson>()
            .RuleFor(request => request.CategoryId, _ => categoryId ?? Guid.CreateVersion7())
            .RuleFor(request => request.Title, faker => faker.Lorem.Sentence(3))
            .RuleFor(request => request.Description, faker => faker.Lorem.Paragraph())
            .RuleFor(request => request.ExternalLink, faker => faker.Internet.Url())
            .RuleFor(request => request.PublicationDate, faker => DateOnly.FromDateTime(faker.Date.Past()))
            .RuleFor(request => request.PaymentDate, faker => DateOnly.FromDateTime(faker.Date.Past()))
            .Generate();
    }
}
