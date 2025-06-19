namespace TaskManager.Interface;

using TaskManager.Domain;
using TaskManager.Domain.Helpers;
public class User
{
    public static async Task<Result<int>> CreateAndSaveUser(
        string email
        , string userName
        , string password
        , string userRole
    )
    {
        // Check if the user does exist 

        Domain.User? user = await Database.UserRepository.ReadUserByEmailAddress(email);

        if (user != null)
            return Result<int>.Failure(new Error(
                "Error.UserCreateError"
                , "Email address already in use"
            ));

        int newUserId = await Database.UserRepository.ReadLatestuserId();

        // Check user password and email

        if (!PasswordValidator.IsValidPassword(password) || !EmailValidator.IsValidEmail(email))
            return Result<int>.Failure(new Error(
                "Error.UserCreateError"
                , "Email and or Password is invalid"
            ));

        // Create user data from input

        UserData userData = new()
        {
            UserId = newUserId,
            UserName = userName,
            UserPassword = password,
            UserEmail = email
        };

        UserRole role = (userRole == "Admin") ? UserRole.Admin : UserRole.User;

        Domain.User newUser = new()
        {
            UserData = userData,
            UserRole = role
        };

        try
        {
            await Database.UserRepository.AddUserAsync(newUser);
            return Result<int>.Success(newUserId);
        }
        catch (Exception e)
        {
            return Result<int>.Failure(new Error("UserCreateError", e.Message));
        }
    }

}
