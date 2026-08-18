using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories.Role;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Role.GetAll;

public class GetAllRolesUseCase : IGetAllRolesUseCase
{
    private readonly IRoleReadOnlyRepository _roleReadOnlyRepository;

    public GetAllRolesUseCase(IRoleReadOnlyRepository roleReadOnlyRepository) => _roleReadOnlyRepository = roleReadOnlyRepository;

    public async Task<ResponseRolesJson> Execute()
    {
        var roles = await _roleReadOnlyRepository.GetAll();

        return new ResponseRolesJson { Roles = roles.Adapt<List<ResponseRoleJson>>() };
    }
}
