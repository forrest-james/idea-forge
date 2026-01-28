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

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("#FFF")]
        [InlineData("#GGGGGG")]
        [InlineData("12345")]
        [InlineData("#12345Z")]
        [InlineData("banana")]
        public void Parse_InvalidHex_Throws(string input)
        {
            Action act = () => HexColor.Parse(input);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Parse_NormalizesToUppercaseWithHash()
        {
            var color = HexColor.Parse("ff5733");

            color.Value.Should().Be("#FF5733");
        }

        [Fact]
        public void TryParse_ValidHex_ReturnsTrueAndValue()
        {
            var success = HexColor.TryParse("#ff5733", out var color);

            success.Should().BeTrue();
            color.Value.Should().Be("#FF5733");
        }

        [Fact]
        public void TryParse_InvalidHex_ReturnsFalseAndDefault()
        {
            var success = HexColor.TryParse("invalid", out var color);

            success.Should().BeFalse();
            color.Value.Should().Be(default);
        }

        [Fact]
        public void Equals_SameHexDifferentCase_AreEqual()
        {
            var a = HexColor.Parse("#ff5733");
            var b = HexColor.Parse("#FF5733");

            a.Should().Be(b);
            (a == b).Should().BeTrue();
        }

        [Fact]
        public void Equals_ObjectOverride_WorksCorrectly()
        {
            object a = HexColor.Parse("#FF5733");
            object b = HexColor.Parse("#ff5733");

            a.Equals(b).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_EqualValues_HaveSameHashCode()
        {
            var a = HexColor.Parse("#FF5733");
            var b = HexColor.Parse("#ff5733");

            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void CanBeUsedAsDictionaryKey()
        {
            var dict = new Dictionary<HexColor, string>();
            dict[HexColor.Parse("#ff5733")] = "primary";

            dict.ContainsKey(HexColor.Parse("#FF5733")).Should().BeTrue();
        }

        [Fact]
        public void ToString_ReturnsNormalizedHex()
        {
            var color = HexColor.Parse("#ff5733");

            color.ToString().Should().Be("#FF5733");
        }
    }
}