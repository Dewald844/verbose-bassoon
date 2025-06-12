using TaskManager;
using Dapper;
using System.Text.Json;

namespace TaskManager.Database;

public class TaskDTO
{
    public required int TaskId;
    public required string Title;
    public required string Description;
    public int CategoryId;
    public required string AssigneeIdL; // Assuming this is a JSON string for multiple assignees
    public DateTime CreatedDate;
    public DateTime DueDate;
    public DateTime CompletedDate;
    public required string TaskStatus;
    public required string TaskPriority;

    public static TaskDTO FromDomain(Domain.Task domainTask)
    {
        return new TaskDTO
        {
            TaskId = domainTask.TaskData.TaskId,
            Title = domainTask.TaskData.Title,
            Description = domainTask.TaskData.Description,
            CategoryId = domainTask.TaskData.CategoryId,
            AssigneeIdL = JsonSerializer.Serialize(domainTask.TaskData.AssigneeIdL),
            CreatedDate = domainTask.TaskData.CreatedDate,
            DueDate = domainTask.TaskData.DueDate,
            CompletedDate = domainTask.TaskData.CompletedDate ?? DateTime.MinValue,
            TaskStatus = domainTask.TaskStatus.ToString(),
            TaskPriority = domainTask.TaskPriority.ToString()
        };
    }
    
    public Domain.Task ToDomain()
    {

        var taskData = new Domain.TaskData
        {
            TaskId = this.TaskId,
            Title = this.Title,
            Description = this.Description,
            CategoryId = this.CategoryId,
            AssigneeIdL = JsonSerializer.Deserialize<HashSet<int>>(this.AssigneeIdL) ?? new HashSet<int>(),
            CreatedDate = this.CreatedDate,
            DueDate = this.DueDate,
            CompletedDate = this.CompletedDate == DateTime.MinValue ? null : this.CompletedDate
        };

        return new Domain.Task
        {
            TaskData = taskData,
            TaskStatus = Enum.Parse<Domain.Status>(TaskStatus),
            TaskPriority = Enum.Parse<Domain.Priority>(TaskPriority)
        };
    }
}

public class TaskRepository
{
    private readonly Controller<TaskDTO> _controller = new();

    public async Task<IEnumerable<Domain.Task>> GetAllTasksAsync()
    {
        string query = "SELECT * FROM Task";
        var result = await _controller.Select(query);
        return result.Select(taskDto => taskDto.ToDomain());
    }

    public async Task<Domain.Task?> GetTaskByIdAsync(int taskId)
    {
        string query = "SELECT * FROM Task WHERE TaskId = @TaskId";
        var tasks = await _controller.Select(query.Replace("@TaskId", taskId.ToString()));
        return tasks.FirstOrDefault()?.ToDomain();
    }

    public async Task AddTaskAsync(Domain.Task task)
    {
        string query = @"INSERT INTO Task 
            (TaskId, Title, Description, CategoryId, AssigneeIdL, CreatedDate, DueDate, CompletedDate, TaskStatus, TaskPriority)
            VALUES (@TaskId, @Title, @Description, @CategoryId, @AssigneeIdL, @CreatedDate, @DueDate, @CompletedDate, @TaskStatus, @TaskPriority)";

        var parameters = TaskDTO.FromDomain(task);

        await _controller.InsertSingle(query, parameters);
    }

    public async Task UpdateTaskAsync(Domain.Task task)
    {
        string query = @"UPDATE Task SET
            Title = @Title,
            Description = @Description,
            CategoryId = @CategoryId,
            AssigneeIdL = @AssigneeIdL,
            CreatedDate = @CreatedDate,
            DueDate = @DueDate,
            CompletedDate = @CompletedDate,
            TaskStatus = @TaskStatus,
            TaskPriority = @TaskPriority
            WHERE TaskId = @TaskId";

        var parameters = TaskDTO.FromDomain(task);

        await _controller.UpdateSingle(query, parameters);
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        string query = "DELETE FROM Task WHERE TaskId = @TaskId";
        await _controller.DeleteSingle(query, taskId);
    }
}