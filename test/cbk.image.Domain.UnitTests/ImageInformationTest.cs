using FluentAssertions;
using cbk.image.Domain.Entity;

namespace cbk.image.Domain.UnitTests
{
    public class ImageInformationTest
    {
        [Test]
        public void Size_WhenWithinLimit_ShouldNotThrowException()
        {
            var imageInformation = new ImageInformation { Size = 5000000 }; // 5MB

            imageInformation.Size.Should().Be(5000000);
        }

        [Test]
        public void Size_WhenExceedsLimit_ShouldThrowException()
        {
            var imageInformation = new ImageInformation();

            Action act = () => imageInformation.Size = 15000000; // 15MB

            act.Should().Throw<ArgumentException>().WithMessage("File size is too large. > 10MB");
        }
    }
}