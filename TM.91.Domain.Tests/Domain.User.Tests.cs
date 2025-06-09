namespace TaskManager.Domain.Tests;

using System.Net;
using TaskManager.Domain;
using TaskManager.Domain.Helpers;
using Xunit;

public class TestData
{
    public static UserData UserTestData { get; set; } = new()
        {
            User_id = 1,
            User_email = "test@test.co.za",
            User_name = "Test test",
            User_password = "Test1234",
        };

    public static UserRole UserAdminRole { get; set; } = UserRole.Admin;

    public User UserAdminWithData { get; set; } = new()
        {
            UserData = UserTestData,
            UserRole = UserAdminRole
        };
}

public class UserTests
{

    [Fact]
    public void TestUserNameUpdate()
    {
        var testUser = new TestData();

        var updateNameR_failure = testUser.UserAdminWithData.UpdateUserName_R("");

        Assert.True(updateNameR_failure.IsFailure);
        Assert.Equal(updateNameR_failure.Error, new Error("Error.InvalidNameString", ""));

        var updateNameR_success = testUser.UserAdminWithData.UpdateUserName_R("Test2");

        Assert.True(updateNameR_success.IsSuccess);
        Assert.Equal("Test2", testUser.UserAdminWithData.UserData.User_name);
    }

    [Fact]
    public void TestUserEmailUpdate()
    {
        var testUser = new TestData();

        var updateEmailR_failure = testUser.UserAdminWithData.UpdateUserEmail_R("");

        Assert.True(updateEmailR_failure.IsFailure);
        Assert.Equal(updateEmailR_failure.Error, new Error("Error.InvalidEmailString", ""));

        var updateEmailR_success = testUser.UserAdminWithData.UpdateUserEmail_R("test2@test.co.za");

        Assert.True(updateEmailR_success.IsSuccess);
        Assert.Equal("test2@test.co.za", testUser.UserAdminWithData.UserData.User_email);
    }

    [Fact]
    public void TestUserPasswordUpdate()
    {
        var testUser = new TestData();

        var updatePasswordR_failure = testUser.UserAdminWithData.UpdateUserPassword_R("", "NewSecureP@ss1");

        Assert.True(updatePasswordR_failure.IsFailure);
        Assert.Equal(updatePasswordR_failure.Error, new Error("Error.InvalidCurrentPassword", ""));

        var updatePasswordR_success = testUser.UserAdminWithData.UpdateUserPassword_R("Test1234", "NewSecureP@ss1");

        Assert.True(updatePasswordR_success.IsSuccess);
        Assert.Equal("NewSecureP@ss1", testUser.UserAdminWithData.UserData.User_password);
    }
}
