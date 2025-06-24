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

public class UserState
{
    public required UserData UserData { get; set; }
    public required UserRole UserRole { get; set; }
}

public class UserCommmand
{
    public static Result<UserState> UpdateUserName_R(string newName, UserState user)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return Result<UserState>.Failure(UserErrors.InvalidNameString(newName));

        user.UserData.UserName = newName;
        return Result<UserState>.Success(user);
    }

    public static Result<UserState> UpdateUserEmail_R(string email, UserState user)
    {
        if (!EmailValidator.IsValidEmail(email))
            return Result<UserState>.Failure(UserErrors.InvalidEmailString(email));

        user.UserData.UserEmail = email;
        return Result<UserState>.Success(user);
    }

    public static Result<UserState> UpdateUserPassword_R(string currentPassword, string newPassword, UserState user)
    {
        if (!PasswordValidator.IsValidPassword(newPassword))
            return Result<UserState>.Failure(UserErrors.InvalidPasswordString(newPassword));

        if (currentPassword != user.UserData.UserPassword)
            return Result<UserState>.Failure(UserErrors.InvalidCurrentPassword(currentPassword));

        user.UserData.UserPassword = newPassword;
        return Result<UserState>.Success(user);
    }
}
