using Application.Features.Palettes.Commands.CreatePalette;
using Application.Features.Palettes.Commands.DeletePalette;
using Application.Features.Palettes.Commands.UpdatePalette;
using Application.Features.Palettes.Queries.GetPaletteIdsInUse;
using Application.Features.Palettes.Queries.ListPalettes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Palettes;

namespace WebApp.Controllers
{
    [Route("palettes")]
    public sealed class PalettesUiController : Controller
    {
        private readonly IMediator _mediator;
        public PalettesUiController(IMediator mediator) => _mediator = mediator;

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var palettes = await _mediator.Send(new ListPaletteQuery(), cancellationToken);
            var inUsePaletteIds = await _mediator.Send(new GetPaletteIdsInUseQuery(), cancellationToken);

            var vm = new PalettesIndexVm
            {
                Palettes = palettes
                .Select(p => new PaletteRowVm
                {
                    Id = p.Id,
                    PrimaryColor = p.PrimaryColor,
                    SecondaryColor = p.SecondaryColor,
                    AccentColor = p.AccentColor,
                    IsInUse = inUsePaletteIds.Contains(p.Id)
                })
                .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PalettesIndexVm form, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreatePaletteCommand
            (
                PrimaryColor: form.PrimaryColor,
                SecondaryColor: form.SecondaryColor,
                AccentColor: form.AccentColor
            ), cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("random")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRandom(CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreatePaletteCommand(), cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id:guid}/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid Id, string primaryColor, string secondaryColor, string accentColor, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdatePaletteCommand
            (
                Id: Id,
                PrimaryColor: primaryColor,
                SecondaryColor: secondaryColor,
                AccentColor: accentColor
            ), cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id:guid}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePaletteCommand(id), cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}