using ProtoBuf.Grpc.Server;
using CompressionLevel = System.IO.Compression.CompressionLevel;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.



// Start: gRPC Configurations
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
//builder.Services.TryAddSingleton(BinderConfiguration.Create(
//    binder: new ServiceBinderWithServiceResolutionFromServiceCollection(builder.Services)));
//builder.Services.AddCodeFirstGrpcReflection();
// End: gRPC Configurations

builder.AddInfrastructureServices();
builder.AddBusinessServices();
builder.Services.AddAuthorization();


var app = builder.Build();
// Configure the HTTP request pipeline.
app.AppUseSwagger();
app.MapGrpcReflectionService();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcServices();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();
