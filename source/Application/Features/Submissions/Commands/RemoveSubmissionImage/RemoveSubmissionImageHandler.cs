using Application.Common.Interfaces;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Submissions.Commands.RemoveSubmissionImage
{
    public sealed class RemoveSubmissionImageHandler : IRequestHandler<RemoveSubmissionImageCommand>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IImageStorage _storage;

        public RemoveSubmissionImageHandler(IAppDbContext dbContext, IImageStorage storage)
        {
            _dbContext = dbContext;
            _storage = storage;
        }

        public async Task Handle(RemoveSubmissionImageCommand request, CancellationToken cancellationToken)
        {
            if (request.SubmissionId == Guid.Empty)
                throw new InvalidOperationException("SubmissionId is required.");

            if (request.ImageId == Guid.Empty)
                throw new InvalidOperationException("ImageId is required.");

            var submission = await _dbContext.Submissions
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == request.SubmissionId, cancellationToken);

            if (submission is null)
                throw new InvalidOperationException("Submission not found.");

            var image = submission.Images.SingleOrDefault(i => i.Id == request.ImageId);
            if (image is null)
                throw new InvalidOperationException("Image not found.");

            var absoluteUrl = image.Url.Value;

            submission.RemoveImage(request.ImageId);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _storage.DeleteByAbsoluteUrl(absoluteUrl, cancellationToken);
        }
    }
}