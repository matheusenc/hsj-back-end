using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.Role;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.Role.DeleteById;

public class DeleteRoleByIdUseCase : IDeleteRoleByIdUseCase
{
    private readonly IRoleReadOnlyRepository _roleReadOnlyRepository;
    private readonly IRoleUpdateOnlyRepository _roleUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleByIdUseCase(
        IRoleReadOnlyRepository roleReadOnlyRepository,
        IRoleUpdateOnlyRepository roleUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _roleReadOnlyRepository = roleReadOnlyRepository;
        _roleUpdateOnlyRepository = roleUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var role = await _roleUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.ROLE_NOT_FOUND);

        if (role.IsSystem)
            throw new ForbiddenAccessException(ErrorMessages.VALIDATION_ROLE_IS_SYSTEM);

        var roleInUse = await _roleReadOnlyRepository.ExistProfileUsingRole(id);
        if (roleInUse)
            throw new ErrorOnValidationException([ErrorMessages.VALIDATION_ROLE_IN_USE]);

        role.Active = false;

        _roleUpdateOnlyRepository.Update(role);
        await _unitOfWork.Commit();
    }
}
