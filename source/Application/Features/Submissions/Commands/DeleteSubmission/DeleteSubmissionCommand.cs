using MediatR;

namespace Application.Features.Submissions.Commands.DeleteSubmission
{
    public sealed record DeleteSubmissionCommand(Guid Id) : IRequest;
}