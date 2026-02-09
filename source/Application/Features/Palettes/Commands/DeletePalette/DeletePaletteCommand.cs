using MediatR;

namespace Application.Features.Palettes.Commands.DeletePalette
{
    public sealed record DeletePaletteCommand(Guid Id) : IRequest;
}