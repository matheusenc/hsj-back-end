using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Document;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IDocumentUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IDocumentUpdateOnlyRepository> _repository = new();

    public IDocumentUpdateOnlyRepositoryBuilder GetById(Document document)
    {
        _repository.Setup(repository => repository.GetById(document.Id)).ReturnsAsync(document);

        return this;
    }

    public IDocumentUpdateOnlyRepository Build() => _repository.Object;
}
