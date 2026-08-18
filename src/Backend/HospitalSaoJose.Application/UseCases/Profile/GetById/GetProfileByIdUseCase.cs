using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Repositories.Profile;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Mapster;

namespace HospitalSaoJose.Application.UseCases.Profile.GetById;

public class GetProfileByIdUseCase : IGetProfileByIdUseCase
{
    private readonly IProfileReadOnlyRepository _profileReadOnlyRepository;

    public GetProfileByIdUseCase(IProfileReadOnlyRepository profileReadOnlyRepository) => _profileReadOnlyRepository = profileReadOnlyRepository;

    public async Task<ResponseProfileJson> Execute(Guid id)
    {
        var profile = await _profileReadOnlyRepository.GetById(id) ?? throw new NotFoundException(ErrorMessages.PROFILE_NOT_FOUND);

        return profile.Adapt<ResponseProfileJson>();
    }
}
