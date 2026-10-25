
namespace PasswordManager;

public class PasswordVault
{
    private readonly Dictionary<string, string> _passwords = new();
    private readonly string _masterPassword;
    private bool _isAuthorized;

    public PasswordVault(string masterPassword)
    {
        _masterPassword = masterPassword;
        _isAuthorized = false;
    }

    public bool Authorize(string masterPassword)
    {
        _isAuthorized = masterPassword == _masterPassword;
        return _isAuthorized;
    }

    public void SavePassword(string website, string password)
    {
        EnsureAuthorized();
        _passwords[website] = password;
    }

    public string? GetPassword(string website)
    {
        EnsureAuthorized();
        return _passwords.GetValueOrDefault(website);
    }

    public void UpdatePassword(string website, string password)
    {
        EnsureAuthorized();

        if (!_passwords.ContainsKey(website))
        {
            throw new KeyNotFoundException(
                $"Password for '{website}' was not found.");
        }

        _passwords[website] = password;
    }

    public bool DeletePassword(string website)
    {
        EnsureAuthorized();
        return _passwords.Remove(website);
    }

    private void EnsureAuthorized()
    {
        if (!_isAuthorized)
        {
            throw new UnauthorizedAccessException(
                "Access denied. Authorize first.");
        }
    }

    public void Logout()
    {
        _isAuthorized = false;
    }
}