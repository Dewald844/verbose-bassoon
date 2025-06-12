using TaskManager;
using Dapper;

namespace TaskManager.Database;

public class UserRepository
{
    private readonly Controller<Domain.User> _controller = new();

    public async Task<IEnumerable<Domain.User>> GetAllUsersAsync()
    {
        string query = "SELECT * FROM User";
        return await _controller.Select(query);
    }

    public async Task<Domain.User?> GetUserByIdAsync(int userId)
    {
        string query = "SELECT * FROM User WHERE UserId = @UserId";
        var users = await _controller.Select(query.Replace("@UserId", userId.ToString()));
        return users.FirstOrDefault();
    }

    public async Task AddUserAsync(Domain.User user)
    {
        string query = @"INSERT INTO User (UserId, UserName, UserPassword, UserEmail, UserRole)
                         VALUES (@UserId, @UserName, @UserPassword, @UserEmail, @UserRole)";
        await _controller.InsertSingle(query, user);
    }

    public async Task UpdateUserAsync(Domain.User user)
    {
        string query = @"UPDATE User SET 
                            UserName = @UserName, 
                            UserPassword = @UserPassword, 
                            UserEmail = @UserEmail, 
                            UserRole = @UserRole
                         WHERE UserId = @UserId";
        await _controller.UpdateSingle(query, user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        string query = "DELETE FROM User WHERE UserId = @UserId";
        await _controller.DeleteSingle(query, userId);
    }
}