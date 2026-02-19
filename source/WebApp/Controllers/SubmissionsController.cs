using Application.Features.Submissions.Commands.CreateSubmission;
using Application.Features.Submissions.Commands.UploadSubmissionImages;
using Application.Features.Submissions.Queries.GetSubmission;
using Application.Features.Submissions.Queries.ListSubmissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public sealed class SubmissionsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubmissionCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var result =  await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(new GetSubmissionQuery(id), cancellationToken);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var items = await mediator.Send(new ListSubmissionsQuery(), cancellationToken);
            return Ok(items);
        }

        [HttpPost("{id:guid}/images")]
        [RequestSizeLimit(50 * 1024 * 1024)] // Total request cap (50MB)
        public async Task<IActionResult> UploadImages(Guid id, [FromForm] List<IFormFile> files, [FromServices] IMediator mediator, CancellationToken cancellationToken)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            await mediator.Send(new UploadSubmissionImagesCommand(
                SubmissionId: id,
                Files: files,
                BaseUrl: baseUrl
                ), cancellationToken);

            return NoContent();
        }
    }
}