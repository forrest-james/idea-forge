namespace Application.Features.Submissions.DTOs
{
    public sealed record SubmissionDto(
        Guid Id,
        string Description,
        ChallengeSummaryDto Challenge,
        IReadOnlyList<SubmissionImageDto> Images,
        string? CreatedBy
    );

    public sealed record SubmissionImageDto(
        Guid Id,
        string ImageUrl,
        int Order
    );

    public sealed record ChallengeSummaryDto(
        Guid Id,
        string Name
    );
}