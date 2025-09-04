using Microsoft.Extensions.DependencyInjection;
using SessionsDB.Repositories;
using SessionsDB.Repositories.Abstractions;

namespace SessionsDB.Configuration
{
    public static class RepositoryConfig
    {
        public static void AddSessionsDBRepositories(this IServiceCollection services)
        {
            services.AddDbContext<SessionContext>();

            services.AddScoped<IMovieRepository, MovieRepository>();
        }
    }
}
