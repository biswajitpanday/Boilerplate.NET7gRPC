using AutoMapper;
using ProtoCore.NET.Core.Interfaces.Services;
using ProtoCore.NET.Proto;

namespace ProtoCore.NET.Api.Services;

public class UserHandler : IProtoUserService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserHandler(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    public async ValueTask<BaseResponse<string>> Create(UserCreateRequest userCreateRequest)
    {
        var user = CustomMapper.Map<UserCreateRequest, UserEntity>(userCreateRequest);
        var response = await _userService.CreateUser(user);
        return BaseResponse<string>.Created(response.ToString());
    }

    public async Task<BaseResponse<List<UserResponse>?>> GetAsync()
    {
        var users = await _userService.GetAsync();
        var mapped = _mapper.Map<List<UserResponse>>(users.ToList());
        return BaseResponse<List<UserResponse>?>.Ok(mapped);
    }

    public async Task<BaseResponse<UserResponse?>> GetByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out _))
            return BaseResponse<UserResponse?>.Failed(null, message: "Invalid ID: It is either empty or not a valid GUID.");
        var user = await _userService.GetAsync(new Guid(id));
        var mapped = _mapper.Map<UserResponse>(user);
        return BaseResponse<UserResponse>.Ok(mapped);
    }
}