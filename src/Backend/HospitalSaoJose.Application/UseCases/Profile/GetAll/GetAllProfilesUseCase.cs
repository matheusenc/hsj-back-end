using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories.Profile;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Profile.GetAll;

public class GetAllProfilesUseCase : IGetAllProfilesUseCase
{
    private readonly IProfileReadOnlyRepository _profileReadOnlyRepository;

    public GetAllProfilesUseCase(IProfileReadOnlyRepository profileReadOnlyRepository) => _profileReadOnlyRepository = profileReadOnlyRepository;

    public async Task<ResponseProfilesJson> Execute()
    {
        var profiles = await _profileReadOnlyRepository.GetAll();

        return new ResponseProfilesJson { Profiles = profiles.Adapt<List<ResponseProfileJson>>() };
    }
}
