namespace WebApp.ViewModels.Challenges
{
    public sealed class CreateSubmissionFormVm
    {
        public string Description { get; set; } = "";
        public List<IFormFile> Files { get; set; } = new();
    }
}