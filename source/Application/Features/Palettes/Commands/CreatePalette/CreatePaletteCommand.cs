using MediatR;

namespace Application.Features.Palettes.Commands.CreatePalette
{
    public sealed record CreatePaletteCommand(
        string PrimaryColor,
        string SecondaryColor,
        string AccentColor
        ) : IRequest<CreatePaletteResult>;
}