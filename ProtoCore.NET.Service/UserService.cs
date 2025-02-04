using ProtoCore.NET.Core.Entities;
using ProtoCore.NET.Core.Interfaces.Repositories;
using ProtoCore.NET.Core.Interfaces.Services;

namespace ProtoCore.NET.Service;

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