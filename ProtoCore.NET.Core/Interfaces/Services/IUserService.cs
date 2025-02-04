using ProtoCore.NET.Core.Entities;

namespace ProtoCore.NET.Core.Interfaces.Services;

public interface IUserService
{
    Task<Guid> CreateUser(UserEntity user);
    Task<IEnumerable<UserEntity>> GetAsync();
    Task<UserEntity> GetAsync(Guid id);
}