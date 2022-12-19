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
    //[OperationContract]
    //Task<string> Create(UserCreateRequest userCreateRequest);

    //[OperationContract]
    [OperationContract]
    ValueTask<UserListResponse> GetAsync();
    
    [OperationContract]
    ValueTask<UserResponse> GetAsync(string id);
}




[ProtoContract]
public class UserResponse : IMapFrom<UserEntity>
{
    //[DataMember(Order = 1)] 
    //public new string Id {
    //    get => Id;
    //    set => Id = value.ToString();
    //}

    [ProtoMember(1)]
    public string? FirstName { get; set; }
    [ProtoMember(2)]
    public string? LastName { get; set; }
    [ProtoMember(3)]
    public string Email { get; set; } = null!;
}


[ProtoContract]
public class UserListResponse : IMapFrom<List<UserEntity>>
{
    public IReadOnlyList<UserResponse> UserResponses;

    [ProtoMember(1, OverwriteList = true)]
    private IEnumerable<UserResponse> _userResponses
    {
        get => UserResponses;
        set => UserResponses = ((IList<UserResponse>)value).AsReadOnly();
    }
}

[ProtoContract]
public class UserCreateRequest
{
    [ProtoMember(1)] 
    public string? FirstName { get; set; }
    [ProtoMember(2)] 
    public string? LastName { get; set; }
}
//[DataContract]
//public class UserCreateRequest
//{
//    [DataMember(Order = 1)]
//    public string FirstName { get; set; } = null!;
//    [DataMember(Order = 2)]
//    public string LastName { get; set; } = null!;
//    [DataMember(Order = 3)]
//    public string Email { get; set; } = null!;
//}