using MediatR;

namespace Application.Features.Palettes.Commands.CreatePalette
{
    public sealed record CreatePaletteCommand(
        string? PrimaryColor = null,
        string? SecondaryColor = null,
        string? AccentColor = null
        ) : IRequest<CreatePaletteResult>;
}