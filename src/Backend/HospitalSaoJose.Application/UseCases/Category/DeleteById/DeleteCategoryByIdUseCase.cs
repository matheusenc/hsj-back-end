using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Category;
using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Category.DeleteById;

public class DeleteCategoryByIdUseCase : IDeleteCategoryByIdUseCase
{
    private readonly ICategoryUpdateOnlyRepository _categoryUpdateOnlyRepository;
    private readonly IDocumentReadOnlyRepository _documentReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryByIdUseCase(
        ICategoryUpdateOnlyRepository categoryUpdateOnlyRepository,
        IDocumentReadOnlyRepository documentReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryUpdateOnlyRepository = categoryUpdateOnlyRepository;
        _documentReadOnlyRepository = documentReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var category = await _categoryUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.CATEGORY_NOT_FOUND);

        var categoryInUse = await _documentReadOnlyRepository.ExistActiveDocumentInCategory(id);
        if (categoryInUse)
            throw new ErrorOnValidationException([ErrorMessages.VALIDATION_CATEGORY_HAS_ACTIVE_DOCUMENTS]);

        category.Active = false;

        _categoryUpdateOnlyRepository.Update(category);
        await _unitOfWork.Commit();
    }
}
