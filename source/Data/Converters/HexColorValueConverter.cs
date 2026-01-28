using Data.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Converters
{
    public sealed class HexColorValueConverter : ValueConverter<HexColor, string>
    {
        public HexColorValueConverter() 
            : base(
                  color => color.Value,
                  value => HexColor.Parse(value))
        {            
        }
    }
}