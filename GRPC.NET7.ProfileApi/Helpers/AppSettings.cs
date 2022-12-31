namespace GRPC.NET7.Profile.Api.Helpers;

public class AppSettings
{
    public string Secret { get; set; } = null!;
    public int Validity { get; set; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}