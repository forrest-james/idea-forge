namespace Application.Features.Palettes.DTOs
{
    public sealed record PaletteDto(
        Guid Id,
        string PrimaryColor,
        string SecondaryColor,
        string AccentColor
        );
}