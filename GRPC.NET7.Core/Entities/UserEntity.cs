using GRPC.NET7.Core.Interfaces.Common;

namespace GRPC.NET7.Core.Entities;

public class UserEntity : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
}