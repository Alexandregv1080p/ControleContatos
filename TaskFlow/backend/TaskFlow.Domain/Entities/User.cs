using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public ICollection<Project> Projects { get; private set; } = new List<Project>();
    public ICollection<TaskItem> AssignedTasks { get; private set; } = new List<TaskItem>();

    // Necessário para o Entity Framework Core.
    private User()
    {
    }

    public User(string name, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("User name is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("User email is required");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("User password hash is required");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
}
