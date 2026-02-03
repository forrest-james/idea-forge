namespace Application.Features.Challenges.DTOs
{
    public sealed record ChallengeListItemDto(
        Guid Id,
        string Name,
        string AppType,
        string AppCategory,
        string PrimaryColor,
        string SecondaryColor,
        string AccentColor
    );
}