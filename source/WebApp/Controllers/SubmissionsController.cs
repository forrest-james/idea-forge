using Application.Features.Submissions.Commands.CreateSubmission;
using Application.Features.Submissions.Queries.GetSubmission;
using Application.Features.Submissions.Queries.ListSubmissions;
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
            var item = await mediator.Send(new GetSubmissionQuery(id), cancellationToken);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("/submissions")]
        public async Task<IActionResult> List([FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var items = await mediator.Send(new ListSubmissionsQuery(), cancellationToken);
            return Ok(items);
        }
    }
}