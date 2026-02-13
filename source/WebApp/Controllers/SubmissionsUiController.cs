using Application.Features.Submissions.Queries.GetSubmission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Submissions;

namespace WebApp.Controllers
{
    [Route("submissions")]
    public class SubmissionsUiController : Controller
    {
        private readonly IMediator _mediator;
        public SubmissionsUiController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new GetSubmissionQuery(id), cancellationToken);
            if (dto is null) return NotFound();

            var vm = new SubmissionDetailsVm
            {
                Id = dto.Id,
                Description = dto.Description,
                ChallengeId = dto.Challenge.Id,
                ChallengeName = dto.Challenge.Name,
                Images = dto.Images
                    .OrderBy(i => i.Order)
                    .Select(i => new SubmissionImageVm
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        Order = i.Order
                    })
                    .ToList(),
                UploadDisabled = true
            };

            return View(vm);
        }
    }
}