using System.Text;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Base;
using Repository.DatabaseContext;
using Serilog;

namespace Api.Helpers;

public static class Extension
{

    #region MiddleWare Configure

    public static void AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        // Swagger, Serilog, DBContext, Identity, Jwt, Authentication, Authorization, <IDatetime, DateTimeService>

        // Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Boilerplate .NET7 Server Using gRPC ",
                Version = "v1",
                Description = "Boilerplate .NET7 Server Using gRPC"
            });
        });

        // Serilog
        builder.Host.UseSerilog((ctx, services, lc) => lc
            .WriteTo.Console()
            .WriteTo.File("Logs\\log-.txt", 
                rollingInterval: RollingInterval.Day, 
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Seq("https://localhost:3999"));

        // Application Database Context
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        });
        
        // Authorization
        builder.Services.AddAuthorization();
    }

    public static void AddBusinessServices(this WebApplicationBuilder builder)
    {
        AddRepositories(builder.Services);
        AddServices(builder.Services);
    }

    #endregion


    #region Private Methods

    public static void AddServices(IServiceCollection services)
    {
        //services.AddTransient<IUserService, UserService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
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