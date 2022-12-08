using GRPC.NET7.Core.Entities;
using GRPC.NET7.Core.Interfaces.Repositories;
using GRPC.NET7.Core.Interfaces.Services;

namespace GRPC.NET7.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> CreateUser(UserEntity user)
    {
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return user.Id;
    }
}