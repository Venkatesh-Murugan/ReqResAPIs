using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace ReqResAPIs.Support;

[TestFixture]
public class BaseAPITest
{
    IConfiguration Configuration => TestConfig.Get();
    protected string? BaseURL => Configuration["ReqResBaseURL"];


}