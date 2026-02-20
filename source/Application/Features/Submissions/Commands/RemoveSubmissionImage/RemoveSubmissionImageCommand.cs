using MediatR;

namespace Application.Features.Submissions.Commands.RemoveSubmissionImage
{
    public sealed record RemoveSubmissionImageCommand(Guid SubmissionId, Guid ImageId) : IRequest;
}