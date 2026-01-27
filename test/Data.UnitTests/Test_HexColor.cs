using Data.ValueObjects;
using FluentAssertions;

namespace Data.UnitTests
{
    public class Test_HexColor
    {
        [Theory]
        [InlineData("#FFFFFF", "#FFFFFF")]
        [InlineData("#000000", "#000000")]
        [InlineData("#ff5733", "#FF5733")]
        [InlineData("FF5733", "#FF5733")]
        public void Parse_ValidHex_CreatesValueObject(string input, string expected)
        {
            var color = HexColor.Parse(input);
            color.Value.Should().Be(expected);
        }


    }
}