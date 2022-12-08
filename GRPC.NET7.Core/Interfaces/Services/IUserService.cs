using GRPC.NET7.Core.Entities;

namespace GRPC.NET7.Core.Interfaces.Services;

public interface IUserService
{
    Task<Guid> CreateUser(UserEntity user);
}