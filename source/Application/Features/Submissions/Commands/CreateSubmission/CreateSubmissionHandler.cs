using Application.Features.Submissions.DTOs;
using Data.Models;
using Data.Persistence;
using Data.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Application.Features.Submissions.Commands.CreateSubmission
{
    public sealed class CreateSubmissionHandler : IRequestHandler<CreateSubmissionCommand, CreateSubmissionResult>
    {
        private readonly IAppDbContext _dbContext;

        public CreateSubmissionHandler(IAppDbContext dbContext) => _dbContext = dbContext;       

        public async Task<CreateSubmissionResult> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
        {
            if (request.ChallengeId == Guid.Empty)
                throw new InvalidOperationException("ChallengeId is required.");

            if (string.IsNullOrWhiteSpace(request.Description))
                throw new InvalidOperationException("Description is required.");            

            var challenge = await _dbContext.Challenges
                .FirstOrDefaultAsync(c => c.Id == request.ChallengeId, cancellationToken);

            if (challenge is null)
                throw new InvalidOperationException("Challenge not found.");

            var submission = new Submission(
                challenge: challenge,
                description: request.Description
            );

            var images = request.Images ?? Array.Empty<CreateSubmissionImageDto>();
            if (images.Count > 0)
            {
                if (images.Select(i => i.Order).Distinct().Count() != images.Count)
                    throw new InvalidOperationException("Image order values must be unique.");

                var normalizedImages = images
                    .OrderBy(i => i.Order)
                    .Select((img, idx) => new { img.ImageUrl, Order = idx })
                    .ToList();

                foreach (var img in normalizedImages)
                    submission.AddImage(ImageUrl.Create(img.ImageUrl));
            }

            _dbContext.Submissions.Add(submission);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateSubmissionResult(submission.Id);
        }
    }
}