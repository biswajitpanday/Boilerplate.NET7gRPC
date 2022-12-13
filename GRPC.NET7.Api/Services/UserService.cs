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
}