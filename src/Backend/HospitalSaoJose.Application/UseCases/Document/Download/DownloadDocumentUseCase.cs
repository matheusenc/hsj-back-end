using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Domain.Storage;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Document.Download;

public class DownloadDocumentUseCase : IDownloadDocumentUseCase
{
    private readonly IDocumentReadOnlyRepository _documentReadOnlyRepository;
    private readonly IFileStorageService _fileStorageService;

    public DownloadDocumentUseCase(IDocumentReadOnlyRepository documentReadOnlyRepository, IFileStorageService fileStorageService)
    {
        _documentReadOnlyRepository = documentReadOnlyRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<DocumentDownload> Execute(Guid id)
    {
        var document = await _documentReadOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.DOCUMENT_NOT_FOUND);

        var content = await _fileStorageService.Get(document.StoredFileName)
            ?? throw new NotFoundException(ErrorMessages.DOCUMENT_FILE_NOT_FOUND);

        return new DocumentDownload(content, document.ContentType, document.OriginalFileName);
    }
}
