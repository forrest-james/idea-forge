namespace WebApp.ViewModels.Palettes
{
    public sealed class PalettesIndexVm
    {
        public List<PaletteRowVm> Palettes { get; init; } = new();

        public string PrimaryColor { get; set; } = "#000000";
        public string SecondaryColor { get; set; } = "#FFFFFF";
        public string AccentColor { get; set; } = "#0000FF";
    }

    public sealed class PaletteRowVm
    {
        public Guid Id { get; init; }
        public string PrimaryColor { get; init; } = "";
        public string SecondaryColor { get; init; } = "";
        public string AccentColor { get; init; } = "";
    }
}