using HospitalSaoJose.Api.Security;
using HospitalSaoJose.Application.UseCases.Profile.DeleteById;
using HospitalSaoJose.Application.UseCases.Profile.GetAll;
using HospitalSaoJose.Application.UseCases.Profile.GetById;
using HospitalSaoJose.Application.UseCases.Profile.Register;
using HospitalSaoJose.Application.UseCases.Profile.UpdateById;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSaoJose.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProfilesController : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ProfilesRead)]
    [ProducesResponseType(typeof(ResponseProfilesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllProfilesUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.ProfilesRead)]
    [ProducesResponseType(typeof(ResponseProfileJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, [FromServices] IGetProfileByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPost]
    [HasPermission(Permissions.ProfilesCreate)]
    [ProducesResponseType(typeof(ResponseRegisteredProfileJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterProfileUseCase useCase,
        [FromBody] RequestProfileJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.ProfilesUpdate)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromServices] IUpdateProfileByIdUseCase useCase,
        [FromBody] RequestProfileJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.ProfilesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromServices] IDeleteProfileByIdUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
