using Microsoft.Extensions.Configuration;

namespace ReqResAPIs.Support;

public class TestConfig
{
    private static string? FindConfigFile() => new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("appsettings." + "*" + ".json").FirstOrDefault()?.FullName;
    public static IConfiguration Get()
    {
        string? currentConfigFile = FindConfigFile();
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(currentConfigFile)
            .Build();
    }
}