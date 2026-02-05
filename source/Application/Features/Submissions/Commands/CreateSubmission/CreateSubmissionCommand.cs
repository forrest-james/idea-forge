using Application.Features.Submissions.DTOs;
using MediatR;

namespace Application.Features.Submissions.Commands.CreateSubmission
{
    public sealed record CreateSubmissionCommand(
        Guid ChallengeId,
        string Description,
        IReadOnlyList<CreateSubmissionImageDto> Images
        ) : IRequest<CreateSubmissionResult>;
}