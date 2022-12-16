using Google.Protobuf.WellKnownTypes;
using GRPC.NET7.Core.Dtos;
using GRPC.NET7.Core.Interfaces.Services;

namespace GRPC.NET7.Api.Services;

public class UserService : User.UserBase
{
    private readonly IUserService _userService;

    public UserService(IUserService userService)
    {
        _userService = userService;
    }
    public override async Task<BaseResponse> Create(UserCreateRequest request, ServerCallContext context)
    {
        //var d = request.DateOfBirth.ToDateTime();
        var user = CustomMapper.Map<UserCreateRequest, UserEntity>(request);
        var response = await _userService.CreateUser(user);
        return ServiceResponse.Created(response.ToString());
    }

    public override async Task<BaseResponse> Get(Empty request, ServerCallContext context)
    {
        var users = await _userService.GetAsync();
        var result = new BaseResponseDto<List<UserEntity>>
        {
            IsSuccess = true,
            Message = string.Empty,
            Data = users.ToList()
        };
        var mapped = CustomMapper.Map<BaseResponseDto<List<UserEntity>>, BaseResponse>(result);
        return mapped;
    }
}