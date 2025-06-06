namespace TaskManager.Domain.Tests;

using TaskManager.Domain.Helpers;
using Xunit;
public class EmailValidatorTests
{
    [Fact]
    public void TestInvalidEmailString()
    {
        var invalidEmailString = "test.com";
        Assert.False(EmailValidator.IsValidEmail(invalidEmailString));
    }

    [Fact]
    public void TestValidEmailString()
    {
        var validEmailString = "test@test.co.za";
        Assert.True(EmailValidator.IsValidEmail(validEmailString));
    }
}

public class PasswordValidatorTests
{
    [Fact]
    public void TestPasswordValidator()
    {
        var password_1_pass = "Passw0rd!";
        Assert.True(PasswordValidator.IsValidPassword(password_1_pass));

        var password_2_fail = "password123";
        Assert.False(PasswordValidator.IsValidPassword(password_2_fail));

        var password_3_fail = "PASSWORD!";
        Assert.False(PasswordValidator.IsValidPassword(password_3_fail));

        var password_4_fail = "P@ss1";
        Assert.False(PasswordValidator.IsValidPassword(password_4_fail));

        var password_5_pass = "SecureP@ss1";
        Assert.True(PasswordValidator.IsValidPassword(password_5_pass));
    }
}