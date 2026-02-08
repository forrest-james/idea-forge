using Application.Features.AppIdeas.Commands.CreateAppIdea;
using Application.Features.AppIdeas.Commands.DeleteAppIdea;
using Application.Features.AppIdeas.Commands.UpdateAppIdea;
using Application.Features.AppIdeas.Queries.GetAppIdea;
using Application.Features.AppIdeas.Queries.ListAppIdeas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public sealed class AppIdeasController : Controller
    {
        [HttpGet("/app-ideas")]
        public async Task<IActionResult> List([FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var items = await mediator.Send(new ListAppIdeasQuery(), cancellationToken);
            return Ok(items);
        }

        [HttpPost("/app-ideas")]
        public async Task<IActionResult> Create([FromBody] CreateAppIdeaCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("/app-ideas/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(new GetAppIdeaQuery(id), cancellationToken);
            return item is null
                ? NotFound()
                : Ok(item);
        }

        [HttpPut("/app-ideas/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppIdeaCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("Id in URL does not match Id in body.");
            await mediator.Send(command with { Id = id }, cancellationToken);
            return NoContent();
        }

        [HttpDelete("/app-ideas/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteAppIdeaCommand(id), cancellationToken);
            return NoContent();
        }
    }
}