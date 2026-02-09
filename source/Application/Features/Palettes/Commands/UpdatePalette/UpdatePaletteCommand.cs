using MediatR;

namespace Application.Features.Palettes.Commands.UpdatePalette
{
    public sealed record UpdatePaletteCommand(
        Guid Id,
        string PrimaryColor,
        string SecondaryColor,
        string AccentColor
        ) : IRequest;
}