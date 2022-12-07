using DotNetCore.Domain;

namespace GRPC.NET7.Core.Entities;

public class BaseEntity : Entity<Guid>
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}