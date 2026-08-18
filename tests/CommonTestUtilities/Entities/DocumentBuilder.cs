using Bogus;
using HospitalSaoJose.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class DocumentBuilder
{
    public static Document Build(Category? category = null)
    {
        var documentCategory = category ?? CategoryBuilder.Build();

        return new Faker<Document>()
            .RuleFor(document => document.Title, faker => faker.Lorem.Sentence(3))
            .RuleFor(document => document.Description, faker => faker.Lorem.Paragraph())
            .RuleFor(document => document.ExternalLink, faker => faker.Internet.Url())
            .RuleFor(document => document.PublicationDate, faker => DateOnly.FromDateTime(faker.Date.Past()))
            .RuleFor(document => document.OriginalFileName, faker => $"{faker.Lorem.Word()}.pdf")
            .RuleFor(document => document.StoredFileName, _ => $"{Guid.CreateVersion7()}.pdf")
            .RuleFor(document => document.ContentType, _ => "application/pdf")
            .RuleFor(document => document.SizeInBytes, faker => faker.Random.Long(1024, 1024 * 1024))
            .RuleFor(document => document.CategoryId, _ => documentCategory.Id)
            .RuleFor(document => document.Category, _ => documentCategory)
            .RuleFor(document => document.CreatedByUserId, _ => Guid.CreateVersion7())
            .Generate();
    }
}
