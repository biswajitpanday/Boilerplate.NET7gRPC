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
builder.Services.AppRegisterSwagger();
// End: gRPC Configurations

builder.AppAddDbContext();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.AppUseSwagger();
app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
