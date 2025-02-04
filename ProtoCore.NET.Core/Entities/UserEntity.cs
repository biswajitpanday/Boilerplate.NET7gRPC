namespace ProtoCore.NET.Core.Entities;

public class UserEntity : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    //public DateTime DateOfBirth { get; set; }
    //public AppEnums.Gender Gender { get; set; }
}