namespace TaskManager.Domain;

using Helpers;


public class TaskErrors : Errors
{
    public static Error InvalidTaskState(string message) =>
        new("Error.InvalidTaskState", message);

    public static Error UserNotPresentInAssigneeL() =>
        new("Error.UserNotPresentInAssigneeList", "User is not present in task assignee list");
}
public class TaskData
{
    public required int TaskId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int CategoryId { get; set; }
    public required HashSet<int> AssigneeIdL { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}

public enum Status { Created, InProgress, Complete }

public enum Priority { Low, Medium, High, Critical }

public class Task
{
    public required Status TaskStatus { get; set; }
    public required Priority TaskPriority { get; set; }
    public required TaskData TaskData { get; set; }
}

public class TaskCommand
{
    public static Result UpdateTaskStatus_R(Task task, Status newStatus)
    {
        task.TaskStatus = newStatus;
        return Result.Success();
    }

    public static Result UpdateTaskPriority_R(Task task, Priority newPriority)
    {
        if (task.TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot update priority"));

        task.TaskPriority = newPriority;
        return Result.Success();
    }

    public static Result AppendAssignedUser_R(Task task, int userId)
    {
        if (task.TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot add assignee"));

        if (task.TaskData.AssigneeIdL.Add(userId))
            return Result.Success();
        else
            return Result.Success();
    }

    public static Result RemoveAssignedUser_R(Task task, int userId)
    {
        if (task.TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot remove assignee"));

        if (task.TaskData.AssigneeIdL.Remove(userId))
            return Result.Success();
        else
            return Result.Failure(TaskErrors.UserNotPresentInAssigneeL());
    }

    public static Result UpdateDueDate_R(Task task, DateTime newDueDate)
    {
        if (task.TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot update due date"));

        task.TaskData.DueDate = newDueDate;
        return Result.Success();
    }

    public static Result CompleteTask_R(Task task, DateTime completeDate)
    {
        if (task.TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already marked as completed"));

        task.TaskData.CompletedDate = completeDate;
        task.TaskStatus = Status.Complete;
        return Result.Success();
    }
}