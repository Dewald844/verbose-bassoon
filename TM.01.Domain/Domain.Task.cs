namespace TaskManager.Domain;

using Helpers;


public class TaskErrors : Errors {
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

    public Result AppendAssignedUser_R(int userId)
    {
        if (TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot add assignee"));
            
        if (TaskData.AssigneeIdL.Add(userId))
            return Result.Success();
        else
            return Result.Success();
    }

    public Result RemoveAssignedUser_R(int userId)
    {
        if (TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot remove assignee"));

        if (TaskData.AssigneeIdL.Remove(userId))
            return Result.Success();
        else
            return Result.Failure(TaskErrors.UserNotPresentInAssigneeL());
    }

    public Result UpdateDueDate_R(DateTime newDueDate)
    {
        if (TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already complete cannot update due date"));

        TaskData.DueDate = newDueDate;
        return Result.Success();
    }

    public Result CompleteTask_R(DateTime completeDate)
    {
        if (TaskStatus is Status.Complete)
            return Result.Failure(TaskErrors.InvalidTaskState("Task is already marked as completed"));

        TaskData.CompletedDate = completeDate;
        TaskStatus = Status.Complete;
        return Result.Success();
    }
}
