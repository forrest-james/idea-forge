using Application.Common.Interfaces;
using Application.Features.Challenges.Commands.CreateChallenge;
using Application.Features.Challenges.Commands.DeleteChallenge;
using Application.Features.Challenges.Commands.UpdateChallengeName;
using Application.Features.Challenges.Queries.GetChallenge;
using Application.Features.Challenges.Queries.GetChallengeIdsWithSubmissions;
using Application.Features.Challenges.Queries.ListChallenges;
using Application.Features.Submissions.Commands.CreateSubmission;
using Application.Features.Submissions.Commands.UploadSubmissionImages;
using Application.Features.Submissions.DTOs;
using Application.Features.Submissions.Queries.ListSubmissionsByChallenge;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Challenges;

namespace WebApp.Controllers
{
    [Route("challenges")]
    public sealed class ChallengesUiController : Controller
    {
        private readonly IMediator _mediator;
        public ChallengesUiController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new ListChallengesQuery(), cancellationToken);
            var inUse = await _mediator.Send(new GetChallengeIdsWithSubmissionsQuery(), cancellationToken);

            var vm = new ChallengesIndexVm
            {
                Challenges = items.Select(c => new ChallengeRowVm
                {
                    Id = c.Id,
                    Name = c.Name,
                    AppType = c.AppType,
                    AppCategory = c.AppCategory,                    
                    PrimaryColor = c.PrimaryColor,
                    SecondaryColor = c.SecondaryColor,
                    AccentColor = c.AccentColor,
                    IsDeletable = !inUse.Contains(c.Id)
                }).ToList()
            };

            return View(vm);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var challenge = await _mediator.Send(new GetChallengeQuery(id), cancellationToken);
            if (challenge is null) return NotFound();

            var subs = await _mediator.Send(new ListSubmissionsByChallengeQuery(id), cancellationToken);

            var vm = new ChallengeDetailsVm
            {
                Id = challenge.Id,
                Name = challenge.Name,
                AppType = challenge.AppIdea.Type.ToString(),
                AppCategory = challenge.AppIdea.Category.ToString(),
                AppDescription = challenge.AppIdea.Description,
                PrimaryColor = challenge.Palette.PrimaryColor.ToString(),
                SecondaryColor = challenge.Palette.SecondaryColor.ToString(),
                AccentColor = challenge.Palette.AccentColor,
                Submissions = subs.ToList()
            };

            return View(vm);
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(CancellationToken cancellationToken)
        {
            var created = await _mediator.Send(new CreateChallengeCommand(), cancellationToken);
            return RedirectToAction(nameof(Details), new { id = created.ChallengeId });
        }

        [HttpPost("{id:guid}/rename")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rename(Guid id, string name, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateChallengeNameCommand(id, name), cancellationToken);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost("{id:guid}/submissions")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubmission(Guid id, [FromForm] CreateSubmissionFormVm form, CancellationToken cancellationToken)
        {
            var createResult = await _mediator.Send(new CreateSubmissionCommand(
                ChallengeId: id,
                Description: form.Description ?? "",
                Images: Array.Empty<CreateSubmissionImageDto>()
                ), cancellationToken);

            var files = form.Files?.Where(f => f is not null && f.Length > 0).ToList() ?? new();
            if (files.Count > 0)
            {
                var uploads = files
                    .Select(f => new ImageUpload(
                        FileName: f.FileName,
                        Content: f.OpenReadStream()))
                    .ToList()
                    .AsReadOnly();

                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                await _mediator.Send(new UploadSubmissionImagesCommand(
                    SubmissionId: createResult.SubmissionId,
                    Files: uploads,
                    BaseUrl: baseUrl
                    ), cancellationToken);
            }

            return Redirect($"/submissions/{createResult.SubmissionId}");
        }

        [HttpPost("{id:guid}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteChallengeCommand(id), cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}