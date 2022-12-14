using System.ServiceModel;
using GRPC.NET7.Core.Entities;
using GRPC.NET7.Core.Interfaces.Common;
using ProtoBuf;

namespace GRPC.NET7.Proto;

public class User
{

}

[ServiceContract]
public interface IProtoUserService
{
    [OperationContract]
    ValueTask<BaseResponse<string>> Create(UserCreateRequest userCreateRequest);

    [OperationContract]
    Task<BaseResponse<UserResponse?>> GetByIdAsync(string id);

    [OperationContract]
    Task<BaseResponse<List<UserResponse>?>> GetAsync();
}


[ProtoContract]
public class UserResponse : IMapFrom<UserEntity>
{
    [ProtoMember(1)]
    public string? FirstName { get; set; }
    [ProtoMember(2)]
    public string? LastName { get; set; }
    [ProtoMember(3)]
    public string Email { get; set; } = null!;
}

[ProtoContract]
public class UserCreateRequest : IMapFrom<UserEntity>
{
    [ProtoMember(1)] 
    public string? FirstName { get; set; }
    [ProtoMember(2)] 
    public string? LastName { get; set; }
    [ProtoMember(3)]
    public string? Email { get; set; }
}