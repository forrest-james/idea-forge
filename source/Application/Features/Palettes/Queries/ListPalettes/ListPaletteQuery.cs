using Application.Features.Palettes.DTOs;
using MediatR;

namespace Application.Features.Palettes.Queries.ListPalettes
{
    public sealed record ListPaletteQuery : IRequest<IReadOnlyList<PaletteDto>>;
}