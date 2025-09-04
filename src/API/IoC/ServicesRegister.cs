using Application;

namespace API.IoC;

public static class ServicesRegister
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IBillboardService, BillboardService>();
    }
}