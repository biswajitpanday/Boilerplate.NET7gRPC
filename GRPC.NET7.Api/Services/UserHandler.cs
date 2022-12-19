using AutoMapper;
using GRPC.NET7.Core.Interfaces.Services;
using GRPC.NET7.Proto;

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
    //public override async Task<BaseResponse> Create(UserCreateRequest request, ServerCallContext context)
    //{
    //    //var d = request.DateOfBirth.ToDateTime();
    //    var user = CustomMapper.Map<UserCreateRequest, UserEntity>(request);
    //    var response = await _userService.CreateUser(user);
    //    return ServiceResponse.Created(response.ToString());
    //}

    //public override async Task<BaseResponse> GetAsync(Empty request, ServerCallContext context)
    //{
    //    var users = await _userService.GetAsync();
    //    var result = new BaseResponseDto<List<UserEntity>>
    //    {
    //        IsSuccess = true,
    //        Message = string.Empty,
    //        Data = users.ToList()
    //    };
    //    var mapped = CustomMapper.Map<BaseResponseDto<List<UserEntity>>, BaseResponse>(result);
    //    return mapped;
    //}
    public async Task<string> Create(UserCreateRequest userCreateRequest)
    {
        var user = CustomMapper.Map<UserCreateRequest, UserEntity>(userCreateRequest);
        var response = await _userService.CreateUser(user);
        //return ServiceResponse.Created(response.ToString());
        return response.ToString();
    }

    public async ValueTask<UserListResponse> GetAsync()
    {
        var users = await _userService.GetAsync();
        var mapped = _mapper.Map<UserListResponse>(users.ToList());
        return (UserListResponse)mapped;
    }

    public async ValueTask<UserResponse> GetAsync(string id)
    {
        var user = await _userService.GetAsync(new Guid(id));
        var mapped = _mapper.Map<UserResponse>(user);
        return mapped;
    }
}