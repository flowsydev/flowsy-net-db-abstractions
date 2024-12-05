using Microsoft.Extensions.Configuration;

namespace Flowsy.Db.Abstractions.Test;

public static class Configuration
{
    public static readonly IConfiguration Instance = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
}