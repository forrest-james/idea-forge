using Application.Features.AppIdeas.Commands.CreateAppIdea;
using Application.Features.AppIdeas.Commands.DeleteAppIdea;
using Application.Features.AppIdeas.Commands.UpdateAppIdea;
using Application.Features.AppIdeas.Queries.GetAppIdeaIdsInUse;
using Application.Features.AppIdeas.Queries.ListAppIdeas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.AppIdeas;

namespace WebApp.Controllers
{
    public sealed class AppIdeasUiController : Controller
    {
        private readonly IMediator _mediator;
        public AppIdeasUiController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var appIdeas = await _mediator.Send(new ListAppIdeasQuery(), cancellationToken);
            var inUseIds = await _mediator.Send(new GetAppIdeaIdsInUseQuery(), cancellationToken);

            var vm = new AppIdeasIndexVm
            {
                AppIdeas = appIdeas.Select(a => new AppIdeaRowVm
                {
                    Id = a.Id,
                    Type = a.Type,
                    Category = a.Category,
                    Description = a.Description,
                    IsInUse = inUseIds.Contains(a.Id)
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppIdeasIndexVm form, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreateAppIdeaCommand(
                Type: form.Type,
                Category: form.Category,
                Description: form.Description
                ), cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, string type, string category, string description, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateAppIdeaCommand(
                Id: id,
                Type: type,
                Category: category,
                Description: description
                ), cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteAppIdeaCommand(id), cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}