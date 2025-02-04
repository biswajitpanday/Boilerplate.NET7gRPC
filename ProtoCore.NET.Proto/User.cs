using System.ServiceModel;
using ProtoCore.NET.Core.Interfaces.Common;
using ProtoBuf;
using ProtoCore.NET.Core.Entities;


namespace ProtoCore.NET.Proto;

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
    public string Id { get; set; } = null!;
    [ProtoMember(2)]
    public string? FirstName { get; set; }
    [ProtoMember(3)]
    public string? LastName { get; set; }
    [ProtoMember(4)]
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