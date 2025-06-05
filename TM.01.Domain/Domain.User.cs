namespace TaskManager.Domain;

public class UserErrors : Errors
{
    public static Error InvalidNameString(string invalidName) => new("Error.InvalidNameString", invalidName);
    public static Error InvalidEmailString(string invalidEmail) => new("Error.InvalidEmailString", invalidEmail);
}

internal class UserData(int id, string name, string password, string email, int companyId)
{
    public required int User_id = id;
    public required string User_name = name;
    public required string User_password = password;
    public required string User_email = email;
    public required int Company_id = companyId;
}

internal enum UserRole { Admin, User }

internal class User(UserData userData, UserRole userRole)
{
    public required UserData UserData { get; set; } = userData;
    public required UserRole UserRole { get; set; } = userRole;
    
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
}
