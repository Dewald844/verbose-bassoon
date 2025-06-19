namespace TaskManager.Domain.Tests;

using System.Net;
using TaskManager.Domain;
using TaskManager.Domain.Helpers;
using Xunit;

public class TestData
{
    public static UserData UserTestData { get; set; } = new()
        {
            UserId = 1,
            UserEmail = "test@test.co.za",
            UserName = "Test test",
            UserPassword = "Test1234",
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

        var updateNameR_failure = User.UpdateUserName_R("", testUser.UserAdminWithData);

        Assert.True(updateNameR_failure.IsFailure);
        Assert.Equal(updateNameR_failure.Error, new Error("Error.InvalidNameString", ""));

        var updateNameR_success = User.UpdateUserName_R("Test2", testUser.UserAdminWithData);

        Assert.True(updateNameR_success.IsSuccess);
        Assert.Equal("Test2", testUser.UserAdminWithData.UserData.UserName);
    }

    [Fact]
    public void TestUserEmailUpdate()
    {
        var testUser = new TestData();

        var updateEmailR_failure = User.UpdateUserEmail_R("", testUser.UserAdminWithData);

        Assert.True(updateEmailR_failure.IsFailure);
        Assert.Equal(updateEmailR_failure.Error, new Error("Error.InvalidEmailString", ""));

        var updateEmailR_success = User.UpdateUserEmail_R("test2@test.co.za", testUser.UserAdminWithData);

        Assert.True(updateEmailR_success.IsSuccess);
        Assert.Equal("test2@test.co.za", testUser.UserAdminWithData.UserData.UserEmail);
    }

    [Fact]
    public void TestUserPasswordUpdate()
    {
        var testUser = new TestData();

        var updatePasswordR_failure = User.UpdateUserPassword_R("", "NewSecureP@ss1", testUser.UserAdminWithData);

        Assert.True(updatePasswordR_failure.IsFailure);
        Assert.Equal(updatePasswordR_failure.Error, new Error("Error.InvalidCurrentPassword", ""));

        var updatePasswordR_success = User.UpdateUserPassword_R("Test1234", "NewSecureP@ss1", testUser.UserAdminWithData);

        Assert.True(updatePasswordR_success.IsSuccess);
        Assert.Equal("NewSecureP@ss1", testUser.UserAdminWithData.UserData.UserPassword);
    }
}
