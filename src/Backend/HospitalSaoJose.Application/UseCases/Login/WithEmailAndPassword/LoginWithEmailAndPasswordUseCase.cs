using HospitalSaoJose.Application.UseCases.Shared;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Domain.Security.PasswordHashing;
using HospitalSaoJose.Domain.Security.Tokens;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginWithEmailAndPasswordUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordHasher passwordHasher,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordHasher = passwordHasher;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseTokensJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);

        if (user is null || _passwordHasher.VerifyPassword(request.Password, user.Password).Equals(false))
            throw new InvalidLoginException();

        var accessToken = _accessTokenGenerator.Generate(user, user.Permissions(), user.IsSuperAdmin());

        return new ResponseTokensJson
        {
            AccessToken = accessToken.Value,
            ExpiresAtUtc = accessToken.ExpiresAtUtc
        };
    }
}
