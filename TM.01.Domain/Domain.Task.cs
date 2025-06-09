namespace TaskManager.Domain;

public class TaskData
{
    public required int Task_id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required HashSet<int> Assignee_idL { get; set; }
    public required int Category_Id { get; set; }
    public required DateTime DueDate { get; set; }
    public DateTime CompletedDate { get; set; }

    public TaskData() {}
}

public enum TaskStatus { Created, InProgress, Complete }
public enum Priority { Low, Medium, High, Critical}