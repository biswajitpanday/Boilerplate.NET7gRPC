using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Base;
using Repository.DatabaseContext;

namespace Api.Helpers;

public static class Extension
{

    #region MiddleWare Configure
    public static void AppRegisterSwagger(this IServiceCollection services)
    {
        services.AddGrpcSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                    { Title = "Boilerplate .NET7 Server Using gRPC ", Version = "v1", Description = "Boilerplate .NET7 Server Using gRPC" });
        });
    }

    public static void AppAddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }
    
    public static void AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        // Swagger, DBContext, Identity, Jwt, Authentication, Authorization, <IDatetime, DateTimeService>
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