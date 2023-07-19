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
            account.Password = "Complex$123";

            // Assert
            account.IsValidPassword(account.Password).Should().BeTrue();
        }

        [Test]
        public void Password_SetWithInvalidComplexity_ShouldThrowException()
        {
            var account = new Account();

            // Act
            account.Password = "simplepass";

            // Assert
            account.IsValidPassword(account.Password).Should().BeFalse();
           
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