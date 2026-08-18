using HospitalSaoJose.Domain.Dtos;
using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Document;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IDocumentReadOnlyRepositoryBuilder
{
    private readonly Mock<IDocumentReadOnlyRepository> _repository = new();

    public IDocumentReadOnlyRepositoryBuilder Filter(List<Document> documents, int totalCount)
    {
        _repository
            .Setup(repository => repository.Filter(It.IsAny<DocumentFilterDto>()))
            .ReturnsAsync(new PagedResult<Document> { Items = documents, TotalCount = totalCount });

        return this;
    }

    public IDocumentReadOnlyRepositoryBuilder GetById(Document document)
    {
        _repository.Setup(repository => repository.GetById(document.Id)).ReturnsAsync(document);

        return this;
    }

    public IDocumentReadOnlyRepositoryBuilder ExistActiveDocumentInCategory(Guid categoryId)
    {
        _repository.Setup(repository => repository.ExistActiveDocumentInCategory(categoryId)).ReturnsAsync(true);

        return this;
    }

    public IDocumentReadOnlyRepository Build() => _repository.Object;
}
