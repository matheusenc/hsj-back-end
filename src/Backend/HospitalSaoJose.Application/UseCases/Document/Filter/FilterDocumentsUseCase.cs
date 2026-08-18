using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Dtos;
using HospitalSaoJose.Domain.Repositories.Document;
using HospitalSaoJose.Exception.ExceptionsBase;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Document.Filter;

public class FilterDocumentsUseCase : IFilterDocumentsUseCase
{
    private readonly IDocumentReadOnlyRepository _documentReadOnlyRepository;

    public FilterDocumentsUseCase(IDocumentReadOnlyRepository documentReadOnlyRepository) => _documentReadOnlyRepository = documentReadOnlyRepository;

    public async Task<ResponseDocumentsJson> Execute(RequestFilterDocumentsJson request)
    {
        ValidateAndThrowOnFailures(request);

        var result = await _documentReadOnlyRepository.Filter(request.Adapt<DocumentFilterDto>());

        return new ResponseDocumentsJson
        {
            Documents = result.Items.Adapt<List<ResponseDocumentJson>>(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)request.PageSize)
        };
    }

    private static void ValidateAndThrowOnFailures(RequestFilterDocumentsJson request)
    {
        var result = new FilterDocumentsValidator().Validate(request);

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
