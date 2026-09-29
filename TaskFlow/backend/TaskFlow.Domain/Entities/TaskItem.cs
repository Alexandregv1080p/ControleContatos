using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

/// <summary>
/// Tarefa de um projeto. Chamada de TaskItem para evitar conflito com System.Threading.Tasks.Task.
/// </summary>
public class TaskItem
{
    public const int TitleMaxLength = 200;

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskItemStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Project? Project { get; private set; }
    public User? AssignedTo { get; private set; }

    // Necessário para o Entity Framework Core.
    private TaskItem()
    {
    }

    public TaskItem(Guid projectId, string title, string? description, TaskPriority priority, DateOnly? dueDate)
    {
        if (projectId == Guid.Empty)
            throw new DomainException("Task project is required");

        Id = Guid.NewGuid();
        ProjectId = projectId;
        Status = TaskItemStatus.Todo;
        CreatedAt = DateTime.UtcNow;
        Update(title, description, priority, dueDate);
    }

    public void Update(string title, string? description, TaskPriority priority, DateOnly? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task title is required");

        if (title.Trim().Length > TitleMaxLength)
            throw new DomainException($"Task title must have at most {TitleMaxLength} characters");

        if (!Enum.IsDefined(priority))
            throw new DomainException("Invalid task priority");

        Title = title.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Priority = priority;
        DueDate = dueDate;
    }

    public void ChangeStatus(TaskItemStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new DomainException("Invalid task status");

        Status = status;
    }

    public void AssignTo(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("Assignee is required");

        AssignedToId = userId;
    }

    public void Unassign() => AssignedToId = null;
}
