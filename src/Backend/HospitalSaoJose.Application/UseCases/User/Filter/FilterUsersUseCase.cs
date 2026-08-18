using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Dtos;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Exception.ExceptionsBase;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.User.Filter;

public class FilterUsersUseCase : IFilterUsersUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public FilterUsersUseCase(IUserReadOnlyRepository userReadOnlyRepository) => _userReadOnlyRepository = userReadOnlyRepository;

    public async Task<ResponseUsersJson> Execute(RequestFilterUsersJson request)
    {
        ValidateAndThrowOnFailures(request);

        var result = await _userReadOnlyRepository.Filter(request.Adapt<UserFilterDto>());

        return new ResponseUsersJson
        {
            Users = result.Items.Adapt<List<ResponseUserJson>>(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)request.PageSize)
        };
    }

    private static void ValidateAndThrowOnFailures(RequestFilterUsersJson request)
    {
        var result = new FilterUsersValidator().Validate(request);

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
