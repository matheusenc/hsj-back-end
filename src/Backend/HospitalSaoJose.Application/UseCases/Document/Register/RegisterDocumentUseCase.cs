using FluentValidation.Results;
using HospitalSaoJose.Application.Mappings;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Identity;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Category;
using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Domain.Storage;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Document.Register;

public class RegisterDocumentUseCase : IRegisterDocumentUseCase
{
    private readonly IDocumentWriteOnlyRepository _documentWriteOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterDocumentUseCase(
        IDocumentWriteOnlyRepository documentWriteOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IFileStorageService fileStorageService,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _documentWriteOnlyRepository = documentWriteOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _fileStorageService = fileStorageService;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredDocumentJson> Execute(RequestDocumentJson request, DocumentFile? file)
    {
        // Higieniza antes de validar, para que as regras enxerguem exatamente
        // o que será gravado.
        request.Description = DocumentDescriptionSanitizer.Sanitize(request.Description);

        await ValidateAndThrowOnFailures(request, file);

        var document = request.Adapt<Domain.Entities.Document>();
        document.CreatedByUserId = _loggedUser.GetUserId();
        // O tipo sai da extensão pela lista de aceitos, e não do cabeçalho que o
        // navegador manda — aquele é escolhido pelo cliente. A validação já
        // garantiu que a extensão está na lista.
        var extensao = file!.Extension();

        document.OriginalFileName = file.FileName;
        document.ContentType = DocumentFileTypes.Find(extensao)!.ContentType;
        document.SizeInBytes = file.SizeInBytes;
        document.StoredFileName = await _fileStorageService.Upload(file.Content, extensao);

        try
        {
            await _documentWriteOnlyRepository.Add(document);
            await _unitOfWork.Commit();
        }
        catch
        {
            // Sem o registro no banco o arquivo vira lixo no disco.
            await _fileStorageService.Delete(document.StoredFileName);
            throw;
        }

        return new ResponseRegisteredDocumentJson
        {
            Id = document.Id,
            Title = document.Title,
            DownloadUrl = MapsterConfiguration.DownloadUrl(document.Id)
        };
    }

    private async Task ValidateAndThrowOnFailures(RequestDocumentJson request, DocumentFile? file)
    {
        var result = new DocumentValidator().Validate(request);

        if (file is null)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_FILE_REQUIRED));
        else
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
