using Data.Models;
using Data.Persistence;
using Data.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Application.Features.Palettes.Commands.CreatePalette
{
    public sealed class CreatePaletteHandler : IRequestHandler<CreatePaletteCommand, CreatePaletteResult>
    {
        private readonly IAppDbContext _dbContext;
        public CreatePaletteHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<CreatePaletteResult> Handle(CreatePaletteCommand request, CancellationToken cancellationToken)
        {
            var generate = string.IsNullOrWhiteSpace(request.PrimaryColor) || 
                           string.IsNullOrWhiteSpace(request.SecondaryColor) || 
                           string.IsNullOrWhiteSpace(request.AccentColor);

            var primaryStr = generate ? RandomHexColor() : request.PrimaryColor!;
            var secondaryStr = generate ? RandomHexColor() : request.SecondaryColor!;
            var accentStr = generate ? RandomHexColor() : request.AccentColor!;

            var primary = HexColor.Parse(primaryStr);
            var secondary = HexColor.Parse(secondaryStr);
            var accent = HexColor.Parse(accentStr);

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

        private static string RandomHexColor()
        {
            Span<byte> bytes = stackalloc byte[3];
            RandomNumberGenerator.Fill(bytes);
            return $"#{bytes[0]:X2}{bytes[1]:X2}{bytes[2]:X2}";
        }
    }
}