using Data.ValueObjects;
using Data.Exceptions;
using FluentAssertions;

namespace Data.UnitTests
{
    public class Test_ImageUrl
    {
        [Theory]
        [InlineData("http://example.com/image.png")]
        [InlineData("http://cdn.example.com/images/1.png")]
        [InlineData("https://example.com/path/to/image.webp")]
        public void Create_ValidAbsoluteUrl_CreatesValueObject(string input)
        {
            var url = ImageUrl.Create(input);

            url.Value.Should().Be(input);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-a-url")]
        [InlineData("/relative/path/image.png")]
        [InlineData("www.example.com/image.jpg")]
        public void Create_InvalidUrl_Throws(string input)
        {
            Action act = () => ImageUrl.Create(input);

            act.Should().Throw<DomainException>().WithMessage("Invalid image URL");
        }

        [Fact]
        public void ToString_ReturnsUrlValue()
        {
            var url = ImageUrl.Create("https://example.com/image.jpg");

            url.ToString().Should().Be("https://example.com/image.jpg");
        }
    }    
}