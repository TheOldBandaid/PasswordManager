
using Xunit;
using System.Collections.Generic;
namespace PasswordManager.Tests;


public class PasswordVaultTests
{
    [Fact]
    public void SavePassword_ShouldStorePasswordForWebsite()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        vault.SavePassword("example.com", "mySecret123");
        Assert.Equal("mySecret123", vault.GetPassword("example.com"));
    }

    [Fact]
    public void GetPassword_ShouldReturnNull_WhenWebsiteDoesNotExist()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        var password = vault.GetPassword("unknown.com");
        Assert.Null(password);
    }

    [Fact]
    public void UpdatePassword_ShouldReplaceExistingPassword()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        vault.SavePassword("example.com", "oldPassword");
        vault.UpdatePassword("example.com", "newPassword");
        Assert.Equal("newPassword", vault.GetPassword("example.com"));
    }

    [Fact]
    public void DeletePassword_ShouldRemoveExistingPassword()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        vault.SavePassword("example.com", "mySecret123");
        vault.DeletePassword("example.com");
        Assert.Null(vault.GetPassword("example.com"));
    }

    [Fact]
    public void UpdatePassword_ShouldThrow_WhenWebsiteDoesNotExist()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        Assert.Throws<KeyNotFoundException>(
            () => vault.UpdatePassword("unknown.com", "newPassword"));
    }

    [Fact]
    public void DeletePassword_ShouldReturnFalse_WhenWebsiteDoesNotExist()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        var result = vault.DeletePassword("unknown.com");
        Assert.False(result);
    }


    [Fact]
    public void GetPassword_ShouldThrow_WhenAccessIsNotAuthorized()
    {
        var vault = new PasswordVault("master123");
        vault.Authorize("master123");
        vault.SavePassword("example.com", "mySecret123");
        vault.Logout();
        Assert.Throws<UnauthorizedAccessException>(
            () => vault.GetPassword("example.com"));
    }

    [Fact]
    public void Authorize_ShouldReturnFalse_WhenMasterPasswordIsIncorrect()
    {
        var vault = new PasswordVault("master123");

        var result = vault.Authorize("wrongPassword");

        Assert.False(result);

        Assert.Throws<UnauthorizedAccessException>(
            () => vault.GetPassword("example.com"));
    }
}
