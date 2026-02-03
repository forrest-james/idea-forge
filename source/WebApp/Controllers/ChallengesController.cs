using Application.Features.Challenges.Commands.CreateChallenge;
using Application.Features.Challenges.Queries.GetChallenge;
using Application.Features.Challenges.Queries.ListChallenges;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ChallengesController : Controller
    {
        [HttpPost("/challenges")]
        public async Task<IActionResult> Create([FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new CreateChallengeCommand(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("/challenges/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var dto = await mediator.Send(new GetChallengeQuery(id), cancellationToken);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpGet("/challenges")]
        public async Task<IActionResult> List([FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var items = await mediator.Send(new ListChallengesQuery(), cancellationToken);
            return Ok(items);            
        }
    }
}