using TaskManager.Domain;
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