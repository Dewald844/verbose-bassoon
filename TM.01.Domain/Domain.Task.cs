namespace TaskManager.Domain;

using TaskManager.Domain.Helpers;


public class TaskErrors : Errors {
    public static Error InvalidTaskState(string message) =>
        new("Error.InvalidTaskState", message);
}
public class TaskData
{
    public required int Task_id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Category_Id { get; set; }
    public required HashSet<int> Assignee_idL { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }

    public TaskData() { }
}

public enum Status { Created, InProgress, Complete }

public enum Priority { Low, Medium, High, Critical }

public class Task
{
    public required Status TaskStatus { get; set; }
    public required Priority TaskPriority { get; set; }
    public required TaskData TaskData { get; set; }

    public Result UpdateTaskStatus_R(Status newStatus)
    {
        TaskStatus = newStatus;
        return Result.Success();
    }

    public Result UpdateTaskPriority_R(Priority newPriority)
    {
        if (TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot update priority"));

        TaskPriority = newPriority;
        return Result.Success();
    }

    public Result AppendAssignedUser(int userId)
    {
        if (TaskData.Assignee_idL.Add(userId))
            return Result.Success();
        else
            return Result.Success();
    }
}

