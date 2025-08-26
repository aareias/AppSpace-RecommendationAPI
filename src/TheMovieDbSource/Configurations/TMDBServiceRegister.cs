using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheMovieDbSource;

public static class TMDBServiceRegister
{
    public static void RegisterTMDBServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITMDBClient, TMDBClient>();
    }
}