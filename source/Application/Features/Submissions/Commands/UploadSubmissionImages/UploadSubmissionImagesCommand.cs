using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Submissions.Commands.UploadSubmissionImages
{
    public sealed record UploadSubmissionImagesCommand(Guid SubmissionId, IReadOnlyList<ImageUpload> Files, string BaseUrl) : IRequest;
}