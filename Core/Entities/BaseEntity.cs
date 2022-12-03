using DotNetCore.Domain;

namespace Core.Entities;

public class BaseEntity : Entity<Guid>
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set;}
}