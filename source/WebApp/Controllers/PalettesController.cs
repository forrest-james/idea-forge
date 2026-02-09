using Application.Features.Palettes.Commands.CreatePalette;
using Application.Features.Palettes.Commands.DeletePalette;
using Application.Features.Palettes.Commands.UpdatePalette;
using Application.Features.Palettes.Queries.GetPalette;
using Application.Features.Palettes.Queries.ListPalettes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public sealed class PalettesController : Controller
    {
        [HttpGet("/palettes")]
        public async Task<IActionResult> List([FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var items = await mediator.Send(new ListPaletteQuery(), cancellationToken);
            return Ok(items);
        }

        [HttpGet("/palettes/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(new GetPaletteQuery(id), cancellationToken);
            return item is null
                ? NotFound()
                : Ok(item);
        }

        [HttpPost("/palettes")]
        public async Task<IActionResult> Create([FromBody] CreatePaletteCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("/palettes/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaletteCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            await mediator.Send(command with { Id = id }, cancellationToken);
            return NoContent();
        }

        [HttpDelete("/palettes/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeletePaletteCommand(id), cancellationToken);
            return NoContent();
        }
    }
}