using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories.Document;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IDocumentWriteOnlyRepositoryBuilder
{
    private readonly Mock<IDocumentWriteOnlyRepository> _repository = new();

    /// <summary>Entidade que o use case mandou persistir, para asserções sobre o que foi montado.</summary>
    public Document? AddedDocument { get; private set; }

    public IDocumentWriteOnlyRepositoryBuilder()
    {
        _repository
            .Setup(repository => repository.Add(It.IsAny<Document>()))
            .Callback<Document>(document => AddedDocument = document)
            .Returns(Task.CompletedTask);
    }

    public IDocumentWriteOnlyRepository Build() => _repository.Object;
}
