using GRPC.NET7.Profile.Core.Entities;
using GRPC.NET7.Profile.Core.Interfaces.Repositories;
using GRPC.NET7.Profile.Core.Interfaces.Services;

namespace GRPC.NET7.Profile.Service;

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

    public async Task<IEnumerable<UserEntity>> GetAsync()
    {
        var users = await _userRepository.ListAsync();
        return users;
    }

    public async Task<UserEntity> GetAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        return user;
    }
}