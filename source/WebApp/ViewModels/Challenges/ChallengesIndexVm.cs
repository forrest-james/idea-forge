namespace WebApp.ViewModels.Challenges
{
    public sealed class ChallengesIndexVm
    {
        public List<ChallengeRowVm> Challenges { get; init; } = new();
    }

    public sealed class ChallengeRowVm
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = "";
        public string AppType { get; init; } = "";
        public string AppCategory { get; init; } = "";
        public string PrimaryColor { get; init; } = "";
        public string SecondaryColor { get; init; } = "";
        public string AccentColor { get; init; } = "";
    }
}