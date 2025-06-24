namespace TaskManager.Interface;

using TaskManager.Domain;
using TaskManager.Domain.Helpers;
public class UserInterface
{

    public static async Task<Result> UpdateUserName(int userId, string newName)
    {
        UserState? user = await Database.UserRepository.ReadUserByIdAsync(userId);

        if (user == null)
            return Result.Failure(new Error(
                "Error.UpdateUserName"
                , "User does not exist int the database"
            ));

        Result<UserState> updateResult = UserCommmand.UpdateUserName_R(newName, user);

        if (updateResult.IsSuccess)
        {
            try
            {
                await Database.UserRepository.UpdateUserAsync(updateResult.Value);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(new Error(
                    "Error.UpdateUserName"
                    , e.Message
                ));
            }
        }
        else
        {
            return Result.Failure(updateResult.Error);
        }
    }

    public static async Task<Result> UpdateUserEmail(int userId, string newEmail)
    {
        UserState? user = await Database.UserRepository.ReadUserByIdAsync(userId);

        if (user == null)
            return Result.Failure(new Error(
                "Error.UpdateUserEmail"
                , "User does not exist int the database"
            ));

        Result<UserState> updateResult = UserCommmand.UpdateUserEmail_R(newEmail, user);

        if (updateResult.IsSuccess)
        {
            try
            {
                await Database.UserRepository.UpdateUserAsync(updateResult.Value);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(new Error(
                    "Error.UpdateUserEmail"
                    , e.Message
                ));
            }
        }
        else
        {
            return Result.Failure(updateResult.Error);
        }
    }

    public static async Task<Result> UpdateUserPassword(
        int userId
        , string currentPassword
        , string newPassword
    ){
        UserState? user = await Database.UserRepository.ReadUserByIdAsync(userId);

        if (user == null)
            return Result.Failure(new Error(
                "Error.UpdateUserPassword"
                , "User does not exist int the database"
            ));

        Result<UserState> updateResult = UserCommmand.UpdateUserPassword_R(
            currentPassword
            , newPassword
            , user
        );

        if (updateResult.IsSuccess)
        {
            try
            {
                await Database.UserRepository.UpdateUserAsync(updateResult.Value);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(new Error(
                    "Error.UpdateUserPassword"
                    , e.Message
                ));
            }
        }
        else
        {
            return Result.Failure(updateResult.Error);
        }
    }

    public static async Task<Result<int>> CreateAndSaveUser(
        string email
        , string userName
        , string password
        , string userRole
    ){
        // Check if the user does exist 

        UserState? user = await Database.UserRepository.ReadUserByEmailAddress(email);

        if (user != null)
            return Result<int>.Failure(new Error(
                "Error.UserCreateError"
                , "Email address already in use"
            ));

        int newUserId = await Database.UserRepository.ReadLatestUserId();

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

        UserState newUser = new()
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
