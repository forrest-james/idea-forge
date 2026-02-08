namespace Application.Features.AppIdeas.DTOs
{
    public sealed record AppIdeaDto(
        Guid Id,
        string Type,
        string Category,
        string Description);
}