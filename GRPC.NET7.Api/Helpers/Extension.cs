using System.Text;
using GRPC.NET7.Core.Interfaces.Services;
using GRPC.NET7.Repository;
using GRPC.NET7.Repository.Base;
using GRPC.NET7.Repository.DatabaseContext;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
using Serilog;
using UserService = GRPC.NET7.Service.UserService;

namespace GRPC.NET7.Api.Helpers;

public static class Extension
{

    #region MiddleWare Configure

    public static void AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        // Swagger, Serilog, DBContext, Identity, Jwt, Authentication, Authorization, <IDatetime, DateTimeService>

        RegisterSwagger(builder);

        RegisterSerilog(builder);

        RegisterDatabaseContext(builder);

        RegisterJwtAuthentication(builder);
        
        builder.Services.AddAuthorization();

        RegisterAutoMapper(builder);
    }
    

    public static void AddBusinessServices(this WebApplicationBuilder builder)
    {
        AddRepositories(builder.Services);
        AddServices(builder);
    }

    #endregion


    #region Private Methods

    public static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Jwt"));
        builder.Services.AddTransient<IUserService, UserService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
    }



    private static void RegisterAutoMapper(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(x => x.FullName!.StartsWith(
                nameof(GRPC.NET7)))); // ToDo: Change "GRPC.NET7" to "YOUR_PROJECT_BASE_NAMESPACE"
    }

    private static void RegisterJwtAuthentication(WebApplicationBuilder builder)
    {
        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        //{
        //    opt.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //        ValidAudience = builder.Configuration["Jwt:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        //    };
        //});
    }

    private static void RegisterDatabaseContext(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }

    private static void RegisterSerilog(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, services, lc) => lc
            .WriteTo.Console()
            .WriteTo.File("Logs\\log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Seq("https://localhost:7166"));
    }

    private static void RegisterSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Boilerplate .NET7 Server Using gRPC ",
                Version = "v1",
                Description = "Boilerplate .NET7 Server Using gRPC"
            });
        });
    }

    #endregion


    #region MiddleWare Use

    public static void AppUseSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Boilerplate .NET7 Server Using gRPC");
            options.RoutePrefix = string.Empty;
        });
    }

    #endregion
}