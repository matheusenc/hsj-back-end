using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Role;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Role.UpdateById;

public class UpdateRoleByIdUseCase : IUpdateRoleByIdUseCase
{
    private readonly IRoleUpdateOnlyRepository _roleUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleByIdUseCase(IRoleUpdateOnlyRepository roleUpdateOnlyRepository, IUnitOfWork unitOfWork)
    {
        _roleUpdateOnlyRepository = roleUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestUpdateRoleJson request)
    {
        ValidateAndThrowOnFailures(request);

        var role = await _roleUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.ROLE_NOT_FOUND);

        if (role.IsSystem)
            throw new ForbiddenAccessException(ErrorMessages.VALIDATION_ROLE_IS_SYSTEM);

        role.Name = request.Name;
        role.Description = request.Description;

        _roleUpdateOnlyRepository.Update(role);
        await _unitOfWork.Commit();
    }

    private static void ValidateAndThrowOnFailures(RequestUpdateRoleJson request)
    {
        var result = new UpdateRoleValidator().Validate(request);

        if (result.IsValid.Equals(false))
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).Distinct().ToList());
    }
}
