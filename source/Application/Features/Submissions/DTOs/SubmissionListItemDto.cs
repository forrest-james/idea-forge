namespace Application.Features.Submissions.DTOs
{
    public sealed record SubmissionListItemDto(
        Guid Id,
        string ChallengeName,
        string DescriptionPreview,
        int ImageCount
    );
}