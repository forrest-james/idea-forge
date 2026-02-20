using Application.Common.Interfaces;
using Data.Persistence;
using Data.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Submissions.Commands.UploadSubmissionImages
{
    public sealed class UploadSubmissionImagesHandler : IRequestHandler<UploadSubmissionImagesCommand>
    {
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp" };

        private const long MaxFileBytes = 10 * 1024 * 1024; // 10MB per file

        private readonly IAppDbContext _dbContext;
        private readonly IImageStorage _storage;

        public UploadSubmissionImagesHandler(IAppDbContext dbContext, IImageStorage storage)
        {
            _dbContext = dbContext;
            _storage = storage;
        }

        public async Task Handle(UploadSubmissionImagesCommand request, CancellationToken cancellationToken)
        {
            var submission = await _dbContext.Submissions
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == request.SubmissionId, cancellationToken);

            if (submission is null)
                throw new InvalidOperationException("Submission not found.");

            var relativePaths = await _storage.SaveSubmissionImages(request.SubmissionId, request.Files, cancellationToken);

            foreach (var path in relativePaths)
            {
                var absoluteUrl = $"{request.BaseUrl}{path}";
                submission.AddImage(ImageUrl.Create(absoluteUrl));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}