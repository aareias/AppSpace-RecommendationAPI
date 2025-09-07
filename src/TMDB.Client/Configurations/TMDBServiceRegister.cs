using Microsoft.Extensions.DependencyInjection;

namespace TMDB.Client.Configurations;

public static class TMDBServiceRegister
{
    public static void RegisterTMDBServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITmdbClient, TmdbClient>();
    }
}