
using System.Text.Json;
using PasswordManager;
using Xunit;

namespace PasswordManager.SDD;

public class PasswordVaultTests4
{
    public static IEnumerable<object[]> SpecificationCases
    {
        get
        {
            var path = Path.Combine(
                AppContext.BaseDirectory,
                "table.json");

            var json = File.ReadAllText(path);

            var cases = JsonSerializer.Deserialize<List<SpecificationCase>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

            return cases.Select(testCase => new object[] { testCase });
        }
    }

    [Theory]
    [MemberData(nameof(SpecificationCases))]
    public void SpecificationCase_MatchesExpectedResult(
        SpecificationCase testCase)
    {
        var vault = new PasswordVault("master123");
        Assert.True(vault.Authorize("master123"));

        if (testCase.InitialSite != null)
        {
            vault.SavePassword(
                testCase.InitialSite,
                testCase.InitialPassword!);
        }

        if (!testCase.Authorized)
        {
            vault.Logout();
        }

        string? actualPassword = null;

        Action action = () =>
        {
            switch (testCase.Operation)
            {
                case "get":
                    actualPassword = vault.GetPassword(testCase.Site);
                    break;

                case "save":
                    vault.SavePassword(
                        testCase.Site,
                        testCase.Password!);

                    actualPassword = vault.GetPassword(testCase.Site);
                    break;

                case "update":
                    vault.UpdatePassword(
                        testCase.Site,
                        testCase.Password!);

                    actualPassword = vault.GetPassword(testCase.Site);
                    break;

                case "delete":
                    vault.DeletePassword(testCase.Site);
                    actualPassword = vault.GetPassword(testCase.Site);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Неизвестная операция: {testCase.Operation}");
            }
        };

        var exception = Record.Exception(action);

        if (testCase.ExpectedException == null)
        {
            Assert.Null(exception);
            Assert.Equal(testCase.ExpectedPassword, actualPassword);
        }
        else
        {
            Assert.NotNull(exception);
            Assert.Equal(
                testCase.ExpectedException,
                exception.GetType().Name);
        }
    }
}

public class SpecificationCase
{
    public string Name { get; set; } = "";
    public string Operation { get; set; } = "";
    public bool Authorized { get; set; }
    public string? InitialSite { get; set; }
    public string? InitialPassword { get; set; }
    public string Site { get; set; } = "";
    public string? Password { get; set; }
    public string? ExpectedPassword { get; set; }
    public string? ExpectedException { get; set; }
}