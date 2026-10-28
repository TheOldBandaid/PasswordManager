using Reqnroll;
using Xunit;
using PasswordManager;

namespace PasswordManager.Tests;

[Binding]
public class PasswordVaultTests3dotnet
{
    private PasswordVault _vault = null!;
    private string? _actualPassword;

    [Given("создано хранилище с мастер-паролем {string}")]
    public void GivenVaultCreated(string masterPassword)
    {
        _vault = new PasswordVault(masterPassword);
    }

    [Given("пользователь авторизован")]
    public void GivenUserAuthorized()
    {
        Assert.True(_vault.Authorize("master123"));
    }

    [Given("пароль {string} сохранён для сайта {string}")]
    public void GivenPasswordSaved(string password, string website)
    {
        _vault.SavePassword(website, password);
    }

    [When("пользователь запрашивает пароль для сайта {string}")]
    public void WhenPasswordRequested(string website)
    {
        _actualPassword = _vault.GetPassword(website);
    }

    [Then("возвращённый пароль равен {string}")]
    public void ThenPasswordReturned(string expectedPassword)
    {
        Assert.Equal(expectedPassword, _actualPassword);
    }
}
