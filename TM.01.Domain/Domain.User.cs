namespace TaskManager.Domain;

using Helpers;

public class UserErrors : Errors
{
    public static Error InvalidNameString(string invalidName) =>
        new("Error.InvalidNameString", invalidName);
    public static Error InvalidEmailString(string invalidEmail) =>
        new("Error.InvalidEmailString", invalidEmail);
    public static Error InvalidPasswordString(string invalidPassword) =>
        new("Error.InvalidPasswordString", invalidPassword);
    public static Error InvalidCurrentPassword(string currentPassword) =>
        new("Error.InvalidCurrentPassword", currentPassword);
}

public class UserData
{
    public required int UserId { get; set; }
    public required string UserName { get; set; }
    public required string UserPassword { get; set; }
    public required string UserEmail { get; set; }
}

public enum UserRole { Admin, User }

public class User
{
    public required UserData UserData { get; set; }
    public required UserRole UserRole { get; set; }

    public static Result<User> UpdateUserName_R(string newName, User user)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return Result<User>.Failure(UserErrors.InvalidNameString(newName));

        user.UserData.UserName = newName;
        return Result<User>.Success(user);
    }

    public static Result<User> UpdateUserEmail_R(string email, User user)
    {
        if (!EmailValidator.IsValidEmail(email))
            return Result<User>.Failure(UserErrors.InvalidEmailString(email));

        user.UserData.UserEmail = email;
        return Result<User>.Success(user);
    }

    public static Result<User> UpdateUserPassword_R(string currentPassword, string newPassword, User user)
    {
        if (!PasswordValidator.IsValidPassword(newPassword))
            return Result<User>.Failure(UserErrors.InvalidPasswordString(newPassword));

        if (currentPassword != user.UserData.UserPassword)
            return Result<User>.Failure(UserErrors.InvalidCurrentPassword(currentPassword));

        user.UserData.UserPassword = newPassword;
        return Result<User>.Success(user);
    }
}
