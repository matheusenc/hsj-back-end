using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Profile;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.User.UpdateById;

public class UpdateUserByIdUseCase : IUpdateUserByIdUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IProfileReadOnlyRepository _profileReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserByIdUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IProfileReadOnlyRepository profileReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _profileReadOnlyRepository = profileReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestUpdateUserJson request)
    {
        var user = await _userUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

        await ValidateAndThrowOnFailures(request, id);

        user.Name = request.Name;
        user.Email = request.Email;
        user.ProfileId = request.ProfileId;

        _userUpdateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task ValidateAndThrowOnFailures(RequestUpdateUserJson request, Guid userId)
    {
        var result = new UpdateUserValidator().Validate(request);

        var emailAlreadyExists = await _userReadOnlyRepository.ExistActiveUserWithEmailForOtherUser(request.Email, userId);
        if (emailAlreadyExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_EMAIL_ALREADY_EXISTS));

        if (request.ProfileId != Guid.Empty)
        {
            var profile = await _profileReadOnlyRepository.GetById(request.ProfileId);
            if (profile is null)
                result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.PROFILE_NOT_FOUND));
        }

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
