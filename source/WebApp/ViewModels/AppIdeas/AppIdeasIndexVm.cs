namespace WebApp.ViewModels.AppIdeas
{
    public sealed class AppIdeasIndexVm
    {
        public List<AppIdeaRowVm> AppIdeas { get; init; } = new();

        public string Type { get; set; } = "";
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public sealed class AppIdeaRowVm
    {
        public Guid Id { get; init; }
        public string Type { get; init; } = "";
        public string Category { get; init; } = "";
        public string Description { get; init; } = "";
        public bool IsInUse { get; init; } = false;
    }
}
