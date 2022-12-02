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

    public static void AppAddDbContext(this IServiceCollection services)
    {
        //services.AddDbContext<AppDbContext>(options =>
        //{
        //    options.UseSqlServer(services.GetConnectionString("DefaultConnection"));
        //});
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