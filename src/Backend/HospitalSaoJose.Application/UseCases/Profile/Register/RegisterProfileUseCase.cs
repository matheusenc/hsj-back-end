using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Profile;
using HospitalSaoJose.Domain.Repositories.Role;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Profile.Register;

public class RegisterProfileUseCase : IRegisterProfileUseCase
{
    private readonly IProfileReadOnlyRepository _profileReadOnlyRepository;
    private readonly IProfileWriteOnlyRepository _profileWriteOnlyRepository;
    private readonly IRoleReadOnlyRepository _roleReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterProfileUseCase(
        IProfileReadOnlyRepository profileReadOnlyRepository,
        IProfileWriteOnlyRepository profileWriteOnlyRepository,
        IRoleReadOnlyRepository roleReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _profileReadOnlyRepository = profileReadOnlyRepository;
        _profileWriteOnlyRepository = profileWriteOnlyRepository;
        _roleReadOnlyRepository = roleReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredProfileJson> Execute(RequestProfileJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var profile = new Domain.Entities.Profile
        {
            Name = request.Name,
            Description = request.Description
        };

        foreach (var roleId in request.RoleIds.Distinct())
            profile.ProfileRoles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = roleId });

        await _profileWriteOnlyRepository.Add(profile);
        await _unitOfWork.Commit();

        return new ResponseRegisteredProfileJson { Id = profile.Id, Name = profile.Name };
    }

    private async Task ValidateAndThrowOnFailures(RequestProfileJson request)
    {
        var result = new ProfileValidator().Validate(request);

        var nameAlreadyExists = await _profileReadOnlyRepository.ExistActiveProfileWithName(request.Name);
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
