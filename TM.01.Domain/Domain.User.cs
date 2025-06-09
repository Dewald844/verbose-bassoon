namespace TaskManager.Domain;

using TaskManager.Domain.Helpers;
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
    public required int User_id { get; set; }
    public required string User_name { get; set; }
    public required string User_password { get; set; }
    public required string User_email { get; set; }

    public UserData() {}
}

public enum UserRole { Admin, User }

public class User
{
    public required UserData UserData { get; set; }
    public required UserRole UserRole { get; set; }
    public User() { }

    public Result UpdateUserName_R(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return Result.Failure(UserErrors.InvalidNameString(newName));

        UserData.User_name = newName;
        return Result.Success();
    }

    public Result UpdateUserEmail_R(string email)
    {
        if (!EmailValidator.IsValidEmail(email))
            return Result.Failure(UserErrors.InvalidEmailString(email));

        UserData.User_email = email;
        return Result.Success();

    }

    public Result UpdateUserPassword_R(string currentPassword, string newPassword)
    {
        if (!PasswordValidator.IsValidPassword(newPassword))
            return Result.Failure(UserErrors.InvalidPasswordString(newPassword));

        if (currentPassword != UserData.User_password)
            return Result.Failure(UserErrors.InvalidCurrentPassword(currentPassword));

        UserData.User_password = newPassword;
        return Result.Success();
    }
}
