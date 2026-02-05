using Application.Features.Submissions.DTOs;
using MediatR;

namespace Application.Features.Submissions.Queries.GetSubmission
{
    public sealed record GetSubmissionQuery(Guid Id) : IRequest<SubmissionDto?>;
}