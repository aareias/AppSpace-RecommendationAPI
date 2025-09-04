using Microsoft.Extensions.DependencyInjection;

namespace TMDB.Configurations;

public static class TMDBServiceRegister
{
    public static void RegisterTMDBServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITmdbClient, TmdbClient>();
    }
}