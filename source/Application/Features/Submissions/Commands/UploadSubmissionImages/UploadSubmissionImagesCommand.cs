using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Submissions.Commands.UploadSubmissionImages
{
    public sealed record UploadSubmissionImagesCommand(Guid SubmissionId, IReadOnlyList<IFormFile> Files, string BaseUrl) : IRequest;
}