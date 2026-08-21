using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Category;
using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Domain.Storage;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Document.UpdateById;

public class UpdateDocumentByIdUseCase : IUpdateDocumentByIdUseCase
{
    private readonly IDocumentUpdateOnlyRepository _documentUpdateOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDocumentByIdUseCase(
        IDocumentUpdateOnlyRepository documentUpdateOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork)
    {
        _documentUpdateOnlyRepository = documentUpdateOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestDocumentJson request, DocumentFile? file)
    {
        var document = await _documentUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.DOCUMENT_NOT_FOUND);

        // Higieniza antes de validar, para que as regras enxerguem exatamente
        // o que será gravado.
        request.Description = DocumentDescriptionSanitizer.Sanitize(request.Description);

        await ValidateAndThrowOnFailures(request, file);

        document.Title = request.Title;
        document.Description = request.Description;
        document.ExternalLink = request.ExternalLink;
        document.PublicationDate = request.PublicationDate;
        document.PaymentDate = request.PaymentDate;
        document.CategoryId = request.CategoryId;

        // O arquivo é opcional na edição: sem arquivo novo, só os metadados mudam.
        var replacedFileName = string.Empty;
        if (file is not null)
        {
            var extensao = file.Extension();

            replacedFileName = document.StoredFileName;
            document.OriginalFileName = file.FileName;
            // Trocar um PDF por uma planilha mudava o arquivo e deixava o tipo
            // antigo gravado, o que fazia o download sair rotulado errado.
            document.ContentType = DocumentFileTypes.Find(extensao)!.ContentType;
            document.SizeInBytes = file.SizeInBytes;
            document.StoredFileName = await _fileStorageService.Upload(file.Content, extensao);
        }

        try
        {
            _documentUpdateOnlyRepository.Update(document);
            await _unitOfWork.Commit();
        }
        catch
        {
            if (file is not null)
                await _fileStorageService.Delete(document.StoredFileName);

            throw;
        }

        if (string.IsNullOrEmpty(replacedFileName).Equals(false))
            await _fileStorageService.Delete(replacedFileName);
    }

    private async Task ValidateAndThrowOnFailures(RequestDocumentJson request, DocumentFile? file)
    {
        var result = new DocumentValidator().Validate(request);

        if (file is not null)
            DocumentFileValidator.Validate(result, file);

        if (request.CategoryId != Guid.Empty)
        {
            var category = await _categoryReadOnlyRepository.GetById(request.CategoryId);
            if (category is null)
                result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.CATEGORY_NOT_FOUND));
        }

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
