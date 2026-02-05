using Application.Features.Submissions.DTOs;
using MediatR;

namespace Application.Features.Submissions.Queries.ListSubmissions
{
    public sealed record ListSubmissionsQuery : IRequest<IReadOnlyList<SubmissionListItemDto>>;
}