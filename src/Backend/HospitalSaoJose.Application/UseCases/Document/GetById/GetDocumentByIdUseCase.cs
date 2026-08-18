using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Document.GetById;

public class GetDocumentByIdUseCase : IGetDocumentByIdUseCase
{
    private readonly IDocumentReadOnlyRepository _documentReadOnlyRepository;

    public GetDocumentByIdUseCase(IDocumentReadOnlyRepository documentReadOnlyRepository) => _documentReadOnlyRepository = documentReadOnlyRepository;

    public async Task<ResponseDocumentJson> Execute(Guid id)
    {
        var document = await _documentReadOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.DOCUMENT_NOT_FOUND);

        return document.Adapt<ResponseDocumentJson>();
    }
}
