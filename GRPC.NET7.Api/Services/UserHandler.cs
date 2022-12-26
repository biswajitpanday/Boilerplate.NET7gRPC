using AutoMapper;
using GRPC.NET7.Core.Interfaces.Services;
using GRPC.NET7.Proto;
using GRPC.NET7.Proto.Helpers;

namespace GRPC.NET7.Api.Services;

public class UserHandler : IProtoUserService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserHandler(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<string> Create(UserCreateRequest userCreateRequest)
    {
        var user = CustomMapper.Map<UserCreateRequest, UserEntity>(userCreateRequest);
        var response = await _userService.CreateUser(user);
        //return ServiceResponse.Created(response.ToString());
        return response.ToString();
    }

    public async ValueTask<List<UserResponse>> GetAsync()
    {
        var users = await _userService.GetAsync();
        var mapped = _mapper.Map<List<UserResponse>>(users.ToList());
        return mapped;
    }

    public async ValueTask<UserResponse> GetByIdAsync(string id)
    {
        var user = await _userService.GetAsync(new Guid(id));
        var mapped = _mapper.Map<UserResponse>(user);
        return mapped;
    }
}