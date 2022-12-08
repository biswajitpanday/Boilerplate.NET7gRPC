using AutoMapper;
using GRPC.NET7.Api.Protos;
using GRPC.NET7.Core.Interfaces.Services;

namespace GRPC.NET7.Api.Services
{
    public class UserService : User.UserBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserService(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        public override async Task<UserCreateResponse> Create(UserCreateRequest request, ServerCallContext context)
        {
            var faker = new Faker();
             var userMap = new UserEntity
             {
                 Email = request.Email ?? faker.Person.Email,
                 FirstName = request.FirstName ?? faker.Person.FirstName,
                 LastName = request.LastName ?? faker.Person.LastName
             };

            //var userMap = _mapper.Map<UserEntity>(request);
            var response = await _userService.CreateUser(userMap);
            
            return await Task.FromResult(new UserCreateResponse
            {
                Response = new BaseResponse
                {
                    IsSuccess = true,
                    Message = $"An User is Created {response}. User : {JsonConvert.SerializeObject(userMap, Formatting.None)}",
                    Data = JsonConvert.SerializeObject(userMap)
                }
            });
        }
    }
}
