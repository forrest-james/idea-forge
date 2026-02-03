using Application.Features.Challenges.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Challenges.Queries.GetChallenge
{
    public sealed class GetChallengeHandler : IRequestHandler<GetChallengeQuery, ChallengeDto?>
    {

        private readonly IAppDbContext _dbContext;

        public GetChallengeHandler(IAppDbContext dbContext) => _dbContext = dbContext;         
        
        public async Task<ChallengeDto?> Handle(GetChallengeQuery request, CancellationToken cancellationToken)
        {
            var challenge = await _dbContext.Challenges
                .Include(c => c.AppIdea)
                .Include(c => c.Palette)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (challenge is null)
                return null;

            return new ChallengeDto(
                Id: challenge.Id,
                Name: challenge.Name,
                AppIdea: new AppIdeaDto(
                    Type: challenge.AppIdea.Type.ToString(),
                    Category: challenge.AppIdea.Category.ToString(),
                    Description: challenge.AppIdea.Description
                ),
                Palette: new PaletteDto(
                    PrimaryColor: challenge.Palette.PrimaryColor.ToString(),
                    SecondaryColor: challenge.Palette.SecondaryColor.ToString(),
                    AccentColor: challenge.Palette.AccentColor.ToString()
                )
             );
        }
    }
}