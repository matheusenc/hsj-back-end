using HospitalSaoJose.Domain.Identity;
using HospitalSaoJose.Domain.Repositories;
using HospitalSaoJose.Domain.Repositories.User;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;

namespace HospitalSaoJose.Application.UseCases.User.Deactivate;

public class DeactivateUserUseCase : IDeactivateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        // Impede que o administrador tranque a si mesmo para fora do painel.
        if (_loggedUser.GetUserId() == id)
            throw new ForbiddenAccessException(ErrorMessages.VALIDATION_ACCESS_DENIED);

        var user = await _userUpdateOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

        user.Active = false;

        _userUpdateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }
}
