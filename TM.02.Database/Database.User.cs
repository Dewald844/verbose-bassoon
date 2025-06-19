using TaskManager;
using Dapper;

namespace TaskManager.Database;

public static class UserRepository
{
    private static readonly Controller<Domain.User> _controller = new();
    private static readonly Controller<int> _idController = new();

    public static async Task<IEnumerable<Domain.User>> GetAllUsersAsync()
    {
        string query = "SELECT * FROM User";
        return await _controller.Select(query);
    }

    public static async Task<Domain.User?> GetUserByIdAsync(int userId)
    {
        string query = "SELECT * FROM User WHERE UserId = @UserId";
        var users = await _controller.Select(query.Replace("@UserId", userId.ToString()));
        return users.FirstOrDefault();
    }

    public static async Task<Domain.User?> ReadUserByEmailAddress(string emailAddress)
    {
        string query = "SELECT * FROM User WHERE UserEmail = @UserEmail";
        var users = await _controller.Select(query.Replace("@UserId", emailAddress));
        return users.FirstOrDefault();
    }

    public static async Task AddUserAsync(Domain.User user)
    {
        string query = @"INSERT INTO User (UserId, UserName, UserPassword, UserEmail, UserRole)
                         VALUES (@UserId, @UserName, @UserPassword, @UserEmail, @UserRole)";
        await _controller.InsertSingle(query, user);
    }

    public static async Task UpdateUserAsync(Domain.User user)
    {
        string query = @"UPDATE User SET 
                            UserName = @UserName, 
                            UserPassword = @UserPassword, 
                            UserEmail = @UserEmail, 
                            UserRole = @UserRole
                         WHERE UserId = @UserId";
        await _controller.UpdateSingle(query, user);
    }

    public static async Task DeleteUserAsync(int userId)
    {
        string query = "DELETE FROM User WHERE UserId = @UserId";
        await _controller.DeleteSingle(query, userId);
    }

    public static async Task<int> ReadLatestuserId()
    {
        string query = "SELECT TOP 1 UserId FROM User ORDER BY UserId Desc";
        var users = await _idController.Select(query);
        return users.FirstOrDefault(0);
    }
}