using Microsoft.Extensions.Configuration;

namespace Shared;

public static class ApplicationSettings
{
    static IConfiguration Configuration { get; set; }
    
    public static void SetConfiguration(IConfiguration configuration) => Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    
    public static string GetValue(string key) => Configuration[key] ?? throw new InvalidOperationException();
}