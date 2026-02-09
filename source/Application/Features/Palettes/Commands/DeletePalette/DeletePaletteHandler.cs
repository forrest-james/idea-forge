using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Palettes.Commands.DeletePalette
{
    public sealed class DeletePaletteHandler : IRequestHandler<DeletePaletteCommand>
    {
        private readonly IAppDbContext _dbContext;
        public DeletePaletteHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Handle(DeletePaletteCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            var inUse = await _dbContext.Challenges
                .AsNoTracking()
                .AnyAsync(c => EF.Property<Guid>(c, "PaletteId") == request.Id, cancellationToken);

            if (inUse)
                throw new InvalidOperationException("Cannot delete Palette because it is referenced by one or more Challenges.");

            var palette = await _dbContext.Palettes.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (palette == null)
                return;

            _dbContext.Palettes.Remove(palette);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}