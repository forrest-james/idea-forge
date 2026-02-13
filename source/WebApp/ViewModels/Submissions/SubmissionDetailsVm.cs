namespace WebApp.ViewModels.Submissions
{
    public sealed class SubmissionDetailsVm
    {
        public Guid Id { get; init; }
        public string Description { get; init; } = "";
        public Guid ChallengeId { get; init; }
        public string ChallengeName { get; init; } = "";
        public List<SubmissionImageVm> Images { get; init; } = new();
        public bool UploadDisabled { get; init; } = true;
    }

    public sealed class SubmissionImageVm
    {
        public Guid Id { get; init; }
        public string ImageUrl { get; init; } = "";
        public int Order { get; init; }
    }
}