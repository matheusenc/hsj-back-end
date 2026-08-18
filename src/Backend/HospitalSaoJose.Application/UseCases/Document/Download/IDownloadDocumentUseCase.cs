namespace HospitalSaoJose.Application.UseCases.Document.Download;

public interface IDownloadDocumentUseCase
{
    Task<DocumentDownload> Execute(Guid id);
}
