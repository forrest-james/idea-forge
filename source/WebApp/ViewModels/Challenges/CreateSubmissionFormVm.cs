namespace WebApp.ViewModels.Challenges
{
    public sealed class CreateSubmissionFormVm
    {
        public string Description { get; set; } = "";
        public List<string> ImageUrls { get; set; } = new();
    }
}