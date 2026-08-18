using HospitalSaoJose.Application.UseCases.Shared;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Identity;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.User.Logged;

public class GetLoggedUserUseCase : IGetLoggedUserUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetLoggedUserUseCase(ILoggedUser loggedUser) => _loggedUser = loggedUser;

    public async Task<ResponseLoggedUserJson> Execute()
    {
        var user = await _loggedUser.Get();

        return new ResponseLoggedUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Profile = user.Profile.Adapt<ResponseProfileSummaryJson>(),
            Permissions = user.Permissions()
        };
    }
}
