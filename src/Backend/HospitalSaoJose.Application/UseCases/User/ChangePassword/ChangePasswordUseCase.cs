using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Identity;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Domain.Security.PasswordHashing;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        ValidateAndThrowOnFailures(request, loggedUser);

        var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id) ?? throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

        user.Password = _passwordHasher.HashPassword(request.NewPassword);

        _userUpdateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private void ValidateAndThrowOnFailures(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordMatches = _passwordHasher.VerifyPassword(request.CurrentPassword, loggedUser.Password);
        if (currentPasswordMatches.Equals(false))
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_CURRENT_PASSWORD_INVALID));

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
