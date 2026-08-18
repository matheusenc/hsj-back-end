using HospitalSaoJose.Api.Security;
using HospitalSaoJose.Application.UseCases.Role.DeleteById;
using HospitalSaoJose.Application.UseCases.Role.GetAll;
using HospitalSaoJose.Application.UseCases.Role.Register;
using HospitalSaoJose.Application.UseCases.Role.UpdateById;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSaoJose.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.RolesRead)]
    [ProducesResponseType(typeof(ResponseRolesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllRolesUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPost]
    [HasPermission(Permissions.RolesCreate)]
    [ProducesResponseType(typeof(ResponseRegisteredRoleJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterRoleUseCase useCase,
        [FromBody] RequestRegisterRoleJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.RolesUpdate)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromServices] IUpdateRoleByIdUseCase useCase,
        [FromBody] RequestUpdateRoleJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.RolesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromServices] IDeleteRoleByIdUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
