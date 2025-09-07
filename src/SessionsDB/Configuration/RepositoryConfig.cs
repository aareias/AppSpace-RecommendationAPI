using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SessionsDB.Repositories;
using SessionsDB.Repositories.Abstractions;
using Utils;

namespace SessionsDB.Configuration
{
    public static class RepositoryConfig
    {
        static string ConnectionString => ApplicationSettings.GetValue<string>(SessionInfoConstants.ConnectionString);
        
        public static void AddSessionsDBRepositories(this IServiceCollection services)
        {
            services.AddDbContext<SessionContext>(options =>
                options.UseSqlServer(ConnectionString));

            services.AddScoped<IMovieRepository, MovieRepository>();
        }
    }
}
