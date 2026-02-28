using Data.ValueObjects;

namespace Data.Models
{
    public class Palette : AuditableEntity
    {
        public Guid Id { get; private set; }
        public HexColor PrimaryColor { get; private set; }
        public HexColor SecondaryColor { get; private set; }
        public HexColor AccentColor { get; private set; }

        private Palette() { }

        public Palette(HexColor primaryColor, HexColor secondaryColor, HexColor accentColor)
        {
            Id = Guid.NewGuid();
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            AccentColor = accentColor;
        }

        public void Update(HexColor primaryColor, HexColor secondaryColor, HexColor accentColor)
        {
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            AccentColor = accentColor;
        }
    }
}