using Application.Features.Palettes.DTOs;
using MediatR;

namespace Application.Features.Palettes.Queries.GetPalette
{
    public sealed record GetPaletteQuery(Guid Id) : IRequest<PaletteDto?>;
}