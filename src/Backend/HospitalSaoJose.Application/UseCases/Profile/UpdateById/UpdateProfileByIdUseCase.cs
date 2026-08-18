using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Profile;
using HospitalSaoJose.Domain.Repositories.Role;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Profile.UpdateById;

public class UpdateProfileByIdUseCase : IUpdateProfileByIdUseCase
{
    private readonly IProfileReadOnlyRepository _profileReadOnlyRepository;
    private readonly IProfileUpdateOnlyRepository _profileUpdateOnlyRepository;
    private readonly IRoleReadOnlyRepository _roleReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileByIdUseCase(
        IProfileReadOnlyRepository profileReadOnlyRepository,
        IProfileUpdateOnlyRepository profileUpdateOnlyRepository,
        IRoleReadOnlyRepository roleReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _profileReadOnlyRepository = profileReadOnlyRepository;
        _profileUpdateOnlyRepository = profileUpdateOnlyRepository;
        _roleReadOnlyRepository = roleReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestProfileJson request)
    {
        var profile = await _profileUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.PROFILE_NOT_FOUND);

        if (profile.IsSystem)
            throw new ForbiddenAccessException(ErrorMessages.VALIDATION_PROFILE_IS_SYSTEM);

        await ValidateAndThrowOnFailures(request, id);

        profile.Name = request.Name;
        profile.Description = request.Description;

        profile.ProfileRoles.Clear();
        foreach (var roleId in request.RoleIds.Distinct())
            profile.ProfileRoles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = roleId });

        _profileUpdateOnlyRepository.Update(profile);
        await _unitOfWork.Commit();
    }

    private async Task ValidateAndThrowOnFailures(RequestProfileJson request, Guid profileId)
    {
        var result = new ProfileValidator().Validate(request);

        var nameAlreadyExists = await _profileReadOnlyRepository.ExistActiveProfileWithNameForOtherProfile(request.Name, profileId);
        if (nameAlreadyExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_PROFILE_NAME_ALREADY_EXISTS));

        var roleIds = request.RoleIds.Distinct().ToList();
        if (roleIds.Count > 0)
        {
            var existingRoles = await _roleReadOnlyRepository.GetByIds(roleIds);
            if (existingRoles.Count != roleIds.Count)
                result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_PROFILE_ROLE_NOT_FOUND));
        }

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
