using FluentAssertions;
using cbk.image.Domain.Entity;

namespace cbk.image.Domain.UnitTests
{
    public class AccountTest
    {
        [Test]
        public void Password_SetWithValidComplexity_ShouldNotThrowException()
        {
            var account = new Account();

            // Act
            Action act = () => account.Password = "Complex$123";

            // Assert
            act.Should().NotThrow<ArgumentException>();
        }

        [Test]
        public void Password_SetWithInvalidComplexity_ShouldThrowException()
        {
            var account = new Account();

            // Act
            Action act = () => account.Password = "simplepass";

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Password does not meet complexity requirements.");
        }

        [Test]
        public void IsActiveAccount_WithRecentLogin_ShouldReturnTrue()
        {
            var account = new Account { LoginTime = DateTime.Now.AddDays(-10) };

            account.IsActiveAccount().Should().BeTrue();
        }

        [Test]
        public void IsActiveAccount_WithOldLogin_ShouldReturnFalse()
        {
            var account = new Account { LoginTime = DateTime.Now.AddMonths(-2) };

            account.IsActiveAccount().Should().BeFalse();
        }
    }
}