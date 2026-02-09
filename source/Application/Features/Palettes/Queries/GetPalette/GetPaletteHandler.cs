using Application.Features.Palettes.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Palettes.Queries.GetPalette
{
    public sealed class GetPaletteHandler : IRequestHandler<GetPaletteQuery, PaletteDto?>
    {
        private readonly IAppDbContext _dbContext;
        public GetPaletteHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<PaletteDto?> Handle(GetPaletteQuery request, CancellationToken cancellationToken)
        {
            var raw = await _dbContext.Palettes
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(p => new { p.Id, p.PrimaryColor, p.SecondaryColor, p.AccentColor })
                .FirstOrDefaultAsync(cancellationToken);

            return raw is null
                ? null
                : new PaletteDto
                (
                    Id: raw.Id,
                    PrimaryColor: raw.PrimaryColor.ToString(),
                    SecondaryColor: raw.SecondaryColor.ToString(),
                    AccentColor: raw.AccentColor.ToString()
                );
        }
    }
}