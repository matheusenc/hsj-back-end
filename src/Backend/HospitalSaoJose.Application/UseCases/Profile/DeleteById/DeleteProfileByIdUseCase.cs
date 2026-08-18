using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Profile;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Profile.DeleteById;

public class DeleteProfileByIdUseCase : IDeleteProfileByIdUseCase
{
    private readonly IProfileUpdateOnlyRepository _profileUpdateOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProfileByIdUseCase(
        IProfileUpdateOnlyRepository profileUpdateOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _profileUpdateOnlyRepository = profileUpdateOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var profile = await _profileUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.PROFILE_NOT_FOUND);

        if (profile.IsSystem)
            throw new ForbiddenAccessException(ErrorMessages.VALIDATION_PROFILE_IS_SYSTEM);

        var profileInUse = await _userReadOnlyRepository.ExistActiveUserWithProfile(id);
        if (profileInUse)
            throw new ErrorOnValidationException([ErrorMessages.VALIDATION_PROFILE_HAS_ACTIVE_USERS]);

        profile.Active = false;

        _profileUpdateOnlyRepository.Update(profile);
        await _unitOfWork.Commit();
    }
}
