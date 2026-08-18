using HospitalSaoJose.Api.Security;
using HospitalSaoJose.Application.UseCases.Category.DeleteById;
using HospitalSaoJose.Application.UseCases.Category.GetAll;
using HospitalSaoJose.Application.UseCases.Category.Register;
using HospitalSaoJose.Application.UseCases.Category.UpdateById;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSaoJose.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseCategoriesJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllCategoriesUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPost]
    [HasPermission(Permissions.CategoriesCreate)]
    [ProducesResponseType(typeof(ResponseRegisteredCategoryJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterCategoryUseCase useCase,
        [FromBody] RequestCategoryJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.CategoriesUpdate)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromServices] IUpdateCategoryByIdUseCase useCase,
        [FromBody] RequestCategoryJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.CategoriesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromServices] IDeleteCategoryByIdUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
