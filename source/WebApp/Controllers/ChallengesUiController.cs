using Application.Features.Challenges.Commands.CreateChallenge;
using Application.Features.Challenges.Queries.GetChallenge;
using Application.Features.Challenges.Queries.ListChallenges;
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
                    AccentColor = c.AccentColor
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
    }
}