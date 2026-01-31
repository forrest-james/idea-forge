namespace Application.Features.Challenges.Commands.CreateChallenge
{
    public sealed record CreateChallengeResult(
        Guid ChallengeId,
        string Name,
        Guid AppIdeaId,
        Guid PaletteId);
}