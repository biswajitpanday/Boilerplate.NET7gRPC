using AutoMapper;
using Grpc.Core;
using GRPC.NET7.Profile.Core.Entities;
using GRPC.NET7.Profile.Core.Interfaces.Services;
using GRPC.NET7.Proto;

namespace GRPC.NET7.Profile.Api.Services;

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
        var user = _mapper.Map<UserEntity>(userCreateRequest);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
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
        if (string.IsNullOrEmpty(id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Parameter"));
        var user = await _userService.GetAsync(new Guid(id));
        var mapped = _mapper.Map<UserResponse>(user);
        return BaseResponse<UserResponse>.Ok(mapped);
    }
}