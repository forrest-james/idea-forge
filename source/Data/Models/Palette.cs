using Data.ValueObjects;

namespace Data.Models
{
    internal class Palette
    {
        public HexColor PrimaryColor { get; set; }
        public HexColor SecondaryColor { get; set; }
        public HexColor AccentColor { get; set; }
    }
}