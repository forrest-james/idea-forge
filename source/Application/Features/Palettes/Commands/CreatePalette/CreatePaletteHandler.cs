using Data.Models;
using Data.Persistence;
using Data.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Palettes.Commands.CreatePalette
{
    public sealed class CreatePaletteHandler : IRequestHandler<CreatePaletteCommand, CreatePaletteResult>
    {
        private readonly IAppDbContext _dbContext;
        public CreatePaletteHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<CreatePaletteResult> Handle(CreatePaletteCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.PrimaryColor) || 
                string.IsNullOrWhiteSpace(request.SecondaryColor) || 
                string.IsNullOrWhiteSpace(request.AccentColor))
            {
                throw new InvalidOperationException("All three colors are required.");
            }

            var primary = HexColor.Parse(request.PrimaryColor);
            var secondary = HexColor.Parse(request.SecondaryColor);
            var accent = HexColor.Parse(request.AccentColor);

            var exists = await _dbContext.Palettes.AnyAsync(p => 
                p.PrimaryColor == primary && 
                p.SecondaryColor == secondary && 
                p.AccentColor == accent, cancellationToken);

            if (exists)
                throw new InvalidOperationException("An identical palette already exists.");

            var palette = new Palette(primary, secondary, accent);

            _dbContext.Palettes.Add(palette);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreatePaletteResult(palette.Id);
        }
    }
}