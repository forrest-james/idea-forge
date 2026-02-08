using Application.Features.Palettes.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Palettes.Queries.ListPalettes
{
    public sealed class ListPalettesHandler : IRequestHandler<ListPaletteQuery, IReadOnlyList<PaletteDto>>
    {
        private readonly IAppDbContext _dbContext;
        public ListPalettesHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<PaletteDto>> Handle(ListPaletteQuery request, CancellationToken cancellationToken)
        {
            var raw = await _dbContext.Palettes
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    p.PrimaryColor,
                    p.SecondaryColor,
                    p.AccentColor
                })
                .ToListAsync(cancellationToken);

            return raw
                .OrderBy(p => p.Id)
                .Select(p => new PaletteDto
                (
                    Id: p.Id,
                    PrimaryColor: p.PrimaryColor.ToString(),
                    SecondaryColor: p.SecondaryColor.ToString(),
                    AccentColor: p.AccentColor.ToString()
                ))
                .ToList();
        }
    }
}