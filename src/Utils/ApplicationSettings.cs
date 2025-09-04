using Microsoft.Extensions.Configuration;

namespace Utils;

public static class ApplicationSettings
{
    static IConfiguration Configuration { get; set; }

    public static void SetConfiguration(IConfiguration configuration) => Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    
    public static T GetValue<T>(string key)
    {
        return Configuration.GetValue<T>(key) ?? throw new KeyNotFoundException(key);
    }

    public static string[] GetArrayValue(string key, char separator = ';') => GetValue<string>(key).Split(separator);
}