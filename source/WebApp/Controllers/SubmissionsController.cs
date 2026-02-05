using Application.Features.Submissions.Commands.CreateSubmission;
using Application.Features.Submissions.Queries.GetSubmission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public sealed class SubmissionsController : Controller
    {
        [HttpPost("/submissions")]
        public async Task<IActionResult> Create([FromBody] CreateSubmissionCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var result =  await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("/submissions/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var dto = await mediator.Send(new GetSubmissionQuery(id), cancellationToken);
            return dto is null ? NotFound() : Ok(dto);
        }
    }
}