using System;
using System.Text;
using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Assignment.Task.Tests
{
    public class AuthHelpersTests
    {
        [Fact]
        public void GetPasswordHash_ShouldReturnConsistentHash()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"PasswordKey", "TestPasswordKey"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var authHelper = new AuthHelpers(configuration);
            var password = "TestPassword";

            // Act
            var hash1 = authHelper.GetPasswordHash(password);
            var hash2 = authHelper.GetPasswordHash(password);

            // Assert
            Assert.NotNull(hash1);
            Assert.Equal(hash1, hash2);
        }
    }
}
