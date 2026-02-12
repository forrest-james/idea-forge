using Application.Features.Submissions.DTOs;

namespace WebApp.ViewModels.Challenges
{
    public sealed class ChallengeDetailsVm
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = "";
        public string AppType { get; init; } = "";
        public string AppCategory { get; init; } = "";
        public string AppDescription { get; init; } = "";
        public string PrimaryColor { get; init; } = "";
        public string SecondaryColor { get; init; } = "";
        public string AccentColor { get; init; } = "";

        public List<SubmissionListItemDto> Submissions { get; init; } = new();
    }
}