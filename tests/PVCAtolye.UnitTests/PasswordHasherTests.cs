using FluentAssertions;
using PVCAtolye.Infrastructure.Identity;

namespace PVCAtolye.UnitTests;

public sealed class PasswordHasherTests
{
    [Fact]
    public void VerifyPasswordShouldReturnTrueForOriginalPassword()
    {
        var hasher = new PasswordHasher();
        var hash = hasher.HashPassword("Admin123!");

        hasher.VerifyPassword("Admin123!", hash).Should().BeTrue();
    }

    [Fact]
    public void VerifyPasswordShouldReturnFalseForWrongPassword()
    {
        var hasher = new PasswordHasher();
        var hash = hasher.HashPassword("Admin123!");

        hasher.VerifyPassword("Wrong123!", hash).Should().BeFalse();
    }
}
