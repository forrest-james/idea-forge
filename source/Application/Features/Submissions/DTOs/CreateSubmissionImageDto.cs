namespace Application.Features.Submissions.DTOs
{
    public sealed record CreateSubmissionImageDto(
        string ImageUrl,
        int Order
    );
}