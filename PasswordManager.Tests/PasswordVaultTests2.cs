
using PasswordManager;
using Xunit;

namespace PasswordManager.Tests;

public class PasswordVaultTests2
{
    [Fact]
    public void UserCannotAccessPasswordWithoutAuthorization()
    {
        var vault = new PasswordVault("master123");
        Assert.Throws<UnauthorizedAccessException>(
            () => vault.GetPassword("example.com"));
    }


    [Fact]
    public void UserCannotAccessPasswordsWithWrongMasterPassword()
    {
        var vault = new PasswordVault("master123");
        var authorized = vault.Authorize("wrongPassword");
        Assert.False(authorized);
        Assert.Throws<UnauthorizedAccessException>(
            () => vault.GetPassword("example.com"));
    }

    [Fact]
    public void UserCannotAccessPasswordsAfterLogout()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        vault.SavePassword("example.com", "mySecret123");
        vault.Logout();
        Assert.Throws<UnauthorizedAccessException>(
            () => vault.GetPassword("example.com"));
    }
}

