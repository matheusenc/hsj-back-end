using HospitalSaoJose.Domain.Storage;
using Moq;

namespace CommonTestUtilities.Storage;

public class IFileStorageServiceBuilder
{
    public const string STORED_FILE_NAME = "arquivo-de-teste.pdf";

    private readonly Mock<IFileStorageService> _service = new();

    public IFileStorageServiceBuilder()
    {
        _service.Setup(item => item.Upload(It.IsAny<Stream>(), It.IsAny<string>())).ReturnsAsync(STORED_FILE_NAME);
        _service.Setup(item => item.Delete(It.IsAny<string>())).Returns(Task.CompletedTask);
    }

    public IFileStorageServiceBuilder Get(string storedFileName, Stream content)
    {
        _service.Setup(item => item.Get(storedFileName)).ReturnsAsync(content);

        return this;
    }

    public IFileStorageService Build() => _service.Object;
}
