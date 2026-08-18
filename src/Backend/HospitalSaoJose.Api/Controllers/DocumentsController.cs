using HospitalSaoJose.Api.Security;
using HospitalSaoJose.Application.UseCases.Document;
using HospitalSaoJose.Application.UseCases.Document.DeleteById;
using HospitalSaoJose.Application.UseCases.Document.Download;
using HospitalSaoJose.Application.UseCases.Document.Filter;
using HospitalSaoJose.Application.UseCases.Document.GetById;
using HospitalSaoJose.Application.UseCases.Document.Register;
using HospitalSaoJose.Application.UseCases.Document.UpdateById;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSaoJose.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDocumentsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Filter(
        [FromServices] IFilterDocumentsUseCase useCase,
        [FromQuery] RequestFilterDocumentsJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseDocumentJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, [FromServices] IGetDocumentByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpGet("{id}/download")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download([FromRoute] Guid id, [FromServices] IDownloadDocumentUseCase useCase)
    {
        var download = await useCase.Execute(id);

        return File(download.Content, download.ContentType, download.FileName);
    }

    [HttpPost]
    [HasPermission(Permissions.DocumentsCreate)]
    [ProducesResponseType(typeof(ResponseRegisteredDocumentJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterDocumentUseCase useCase,
        [FromForm] RequestDocumentJson request,
        IFormFile? file)
    {
        var response = await useCase.Execute(request, DocumentFileFrom(file));

        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.DocumentsUpdate)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromServices] IUpdateDocumentByIdUseCase useCase,
        [FromForm] RequestDocumentJson request,
        IFormFile? file)
    {
        await useCase.Execute(id, request, DocumentFileFrom(file));

        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DocumentsDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromServices] IDeleteDocumentByIdUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    /// <summary>
    /// Converte o <see cref="IFormFile"/> em um DTO neutro para a Application não depender do ASP.NET.
    /// </summary>
    private static DocumentFile? DocumentFileFrom(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return null;

        return new DocumentFile(file.OpenReadStream(), Path.GetFileName(file.FileName), file.Length);
    }
}
