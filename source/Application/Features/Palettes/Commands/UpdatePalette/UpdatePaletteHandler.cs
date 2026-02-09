using Data.Persistence;
using Data.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Palettes.Commands.UpdatePalette
{
    public sealed class UpdatePaletteHandler : IRequestHandler<UpdatePaletteCommand>
    {
        private readonly IAppDbContext _dbContext;
        public UpdatePaletteHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Handle(UpdatePaletteCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            var primary = HexColor.Parse(request.PrimaryColor);
            var secondary = HexColor.Parse(request.SecondaryColor);
            var accent = HexColor.Parse(request.AccentColor);

            var palette = await _dbContext.Palettes.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (palette is null)
                throw new InvalidOperationException("Palette not found.");

            palette.Update(primary, secondary, accent);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}