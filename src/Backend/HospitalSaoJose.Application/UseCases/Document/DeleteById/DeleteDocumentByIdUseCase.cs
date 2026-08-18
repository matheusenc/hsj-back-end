using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Document.DeleteById;

public class DeleteDocumentByIdUseCase : IDeleteDocumentByIdUseCase
{
    private readonly IDocumentUpdateOnlyRepository _documentUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentByIdUseCase(IDocumentUpdateOnlyRepository documentUpdateOnlyRepository, IUnitOfWork unitOfWork)
    {
        _documentUpdateOnlyRepository = documentUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var document = await _documentUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.DOCUMENT_NOT_FOUND);

        // Soft delete: o PDF permanece no storage para permitir reativação futura.
        document.Active = false;

        _documentUpdateOnlyRepository.Update(document);
        await _unitOfWork.Commit();
    }
}
