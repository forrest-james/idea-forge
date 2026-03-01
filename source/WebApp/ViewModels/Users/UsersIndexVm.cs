namespace WebApp.ViewModels.Users
{
    public sealed class UsersIndexVm
    {
        public List<UserRowVm> Users { get; init; } = new();
        public List<string> Roles { get; init; } = new();

        public string NewEmail { get; set; } = "";
        public string NewRole { get; set; } = "Standard";
    }
}