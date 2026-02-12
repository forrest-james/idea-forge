using MediatR;

namespace Application.Features.Palettes.Queries.GetPaletteIdsInUse
{
    public sealed record GetPaletteIdsInUseQuery : IRequest<HashSet<Guid>>;
}