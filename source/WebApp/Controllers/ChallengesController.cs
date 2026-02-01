using Application.Features.Challenges.Commands.CreateChallenge;
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
    }
}