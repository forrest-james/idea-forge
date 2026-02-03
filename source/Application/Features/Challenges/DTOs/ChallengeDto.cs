namespace Application.Features.Challenges.DTOs
{
    public sealed record ChallengeDto(
        Guid Id,
        string Name,
        AppIdeaDto AppIdea,
        PaletteDto Palette
        );

    public sealed record AppIdeaDto(
        string Type,
        string Category,
        string Description
        );

    public sealed record PaletteDto(
        string PrimaryColor,
        string SecondaryColor,
        string AccentColor
        );
}