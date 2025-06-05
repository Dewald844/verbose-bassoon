namespace TaskManager.Domain.Tests;

using TaskManager.Domain;
using TaskManager.Domain.Helpers;
using Xunit;

public class UserTests
{

    [Fact]
    public void TestUserNameUpdate()
    {
        var userTestData = new UserData
        {
            User_id = 1,
            User_email = "test@test.co.za",
            User_name = "Test test",
            User_password = "Test1234",
            Company_id = 1
        };

        var userTestRole = UserRole.Admin;

        var userTest = new User
        {
            UserData = userTestData,
            UserRole = userTestRole
        };

        var updateNameR_failure = userTest.UpdateUserName_R("");
        var updateNameR_success = userTest.UpdateUserName_R("Test2");

        Assert.True(updateNameR_failure.IsFailure);
        Assert.Equal(updateNameR_failure.Error, new Error("Error.InvalidNameString", ""));
        
        Assert.True(updateNameR_success.IsSuccess);
        Assert.Equal("Test2", userTest.UserData.User_name);
    }
}
