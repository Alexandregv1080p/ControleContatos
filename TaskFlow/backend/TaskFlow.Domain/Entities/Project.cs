using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class Project
{
    public const int NameMaxLength = 150;

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User? Owner { get; private set; }
    public ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();

    // Necessário para o Entity Framework Core.
    private Project()
    {
    }

    public Project(string name, string? description, Guid ownerId)
    {
        if (ownerId == Guid.Empty)
            throw new DomainException("Project owner is required");

        Id = Guid.NewGuid();
        OwnerId = ownerId;
        CreatedAt = DateTime.UtcNow;
        Update(name, description);
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Project name is required");

        if (name.Trim().Length > NameMaxLength)
            throw new DomainException($"Project name must have at most {NameMaxLength} characters");

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }
}
