using Application.Features.AppIdeas.Commands;
using Application.Features.AppIdeas.Queries;
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
    }
}