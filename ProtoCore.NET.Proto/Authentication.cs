using System.Runtime.Serialization;
using System.ServiceModel;
using ProtoBuf.Grpc;

namespace ProtoCore.NET.Proto
{
    public class Authentication
    {

    }

    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, CallContext context = default);
    }

    [DataContract]
    public class AuthenticationRequest
    {
        [DataMember(Order = 1)]
        public string UserName { get; set; } = null!;
        [DataMember(Order = 2)]
        public string Password { get; set; } = null!;
    }

    [DataContract]
    public class AuthenticationResponse
    {
        [DataMember(Order = 1)]
        public string AccessToken { get; set; } = null!;
        [DataMember(Order = 2)]
        public int ExpiresIn { get; set; }
    }
}