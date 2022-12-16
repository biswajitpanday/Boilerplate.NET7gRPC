using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GRPC.NET7.Proto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GRPC.NET7.Api.Helpers;

public static class JwtAuthenticationManager
{
    public static AuthenticationResponse Authenticate(IOptions<AppSettings> appSettings, AuthenticationRequest authenticationRequest)
    {
        // Implement DbCheck ----
        if (authenticationRequest.UserName != "admin" || authenticationRequest.Password != "admin")
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid User Credentials"));

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(appSettings.Value.Secret);
        var tokenExpiryDateTime = DateTime.UtcNow.AddMinutes(appSettings.Value.Validity);
        var securityTokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, authenticationRequest.UserName),
                new(ClaimTypes.Role, "Administrator")
            }),
            Expires = tokenExpiryDateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        
        return new AuthenticationResponse
        {
            AccessToken = token,
            ExpiresIn = (int)tokenExpiryDateTime.Subtract(DateTime.UtcNow).TotalSeconds
        };
    }
}