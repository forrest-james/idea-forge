using Application.Common.Interfaces;
using Application.Features.Submissions.Commands.DeleteSubmission;
using Application.Features.Submissions.Commands.RemoveSubmissionImage;
using Application.Features.Submissions.Commands.UploadSubmissionImages;
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

        [HttpPost("{id:guid}/images")]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> UploadImages(Guid id, List<IFormFile> files, CancellationToken cancellationToken)
        {
            var uploads = files
                .Where(f => f.Length > 0)
                .Select(f => new ImageUpload(
                    FileName: f.FileName,
                    Content: f.OpenReadStream()))
                .ToList();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            await _mediator.Send(new UploadSubmissionImagesCommand(
                SubmissionId: id,
                Files: uploads,
                BaseUrl: baseUrl
                ), cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost("{id:guid}/images/{imageId:guid}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(Guid id, Guid imageId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveSubmissionImageCommand(
                SubmissionId: id,
                ImageId: imageId
                ), cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost("{id:guid}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new GetSubmissionQuery(id), cancellationToken);
            if (dto is null) return NotFound();

            await _mediator.Send(new DeleteSubmissionCommand(id), cancellationToken);

            return Redirect($"/challenges/{dto.Challenge.Id}");
        }
    }
}