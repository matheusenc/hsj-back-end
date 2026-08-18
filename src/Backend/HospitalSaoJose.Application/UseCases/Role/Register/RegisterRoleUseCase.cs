using FluentValidation.Results;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Role;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Role.Register;

public class RegisterRoleUseCase : IRegisterRoleUseCase
{
    private readonly IRoleReadOnlyRepository _roleReadOnlyRepository;
    private readonly IRoleWriteOnlyRepository _roleWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRoleUseCase(
        IRoleReadOnlyRepository roleReadOnlyRepository,
        IRoleWriteOnlyRepository roleWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _roleReadOnlyRepository = roleReadOnlyRepository;
        _roleWriteOnlyRepository = roleWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredRoleJson> Execute(RequestRegisterRoleJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var role = request.Adapt<Domain.Entities.Role>();

        await _roleWriteOnlyRepository.Add(role);
        await _unitOfWork.Commit();

        return new ResponseRegisteredRoleJson { Id = role.Id, Key = role.Key };
    }

    private async Task ValidateAndThrowOnFailures(RequestRegisterRoleJson request)
    {
        var result = new RegisterRoleValidator().Validate(request);

        var keyAlreadyExists = await _roleReadOnlyRepository.ExistActiveRoleWithKey(request.Key);
        if (keyAlreadyExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_ROLE_KEY_ALREADY_EXISTS));

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
