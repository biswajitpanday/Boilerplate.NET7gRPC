using System.IO.Compression;
using GRPC.NET7.Profile.Api.Helpers;
using GRPC.NET7.Profile.Api.Interceptors;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<LoggerInterceptor>();
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddCodeFirstGrpc(config =>
{
    config.ResponseCompressionLevel = CompressionLevel.Optimal;
});

builder.AddInfrastructureServices();
builder.AddBusinessServices();
builder.Services.AddAuthorization();

var app = builder.Build();
app.AppUseSwagger();
app.MapGrpcReflectionService();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcServices();
// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/profileApi", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
