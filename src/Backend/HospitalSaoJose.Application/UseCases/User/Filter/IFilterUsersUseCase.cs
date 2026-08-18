using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;

namespace HospitalSaoJose.Application.UseCases.User.Filter;

public interface IFilterUsersUseCase
{
    Task<ResponseUsersJson> Execute(RequestFilterUsersJson request);
}
