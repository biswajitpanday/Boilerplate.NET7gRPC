using Microsoft.OpenApi.Models;

namespace Boilerplate.NET7gRPC.Extensions;

public static class Extension
{

    #region MiddleWare Configure
    /// <summary>
    /// Configure Swagger for this application
    /// </summary>
    /// <param name="services"></param>
    public static void AppRegisterSwagger(this IServiceCollection services)
    {
        services.AddGrpcSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                    { Title = "Hedwig Server", Version = "v1", Description = "Boilerplate .NET7 Server Using gRPC" });
        });
    }

    #endregion



    #region MiddleWare Use

    /// <summary>
    /// Use Swagger and make Swagger url the main api url
    /// </summary>
    /// <param name="app"></param>
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