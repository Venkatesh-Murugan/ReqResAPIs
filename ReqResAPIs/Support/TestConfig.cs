using Microsoft.Extensions.Configuration;

namespace ReqResAPIs.Support;

public class TestConfig
{
    //private static string FindConfigFile() => new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("appsettings.json").FirstOrDefault().FullName;

    public static IConfiguration Get()
    {
        var currentConfigFile = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .Build();
        return currentConfigFile;
    }
}