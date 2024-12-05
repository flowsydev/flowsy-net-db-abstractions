using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Flowsy.Db.Abstractions.Test.Fixtures;

public class PostgresDatabaseFixture : IDisposable
{
    private readonly PostgreSqlContainer _container;
    
    public string ConnectionString => _container.GetConnectionString();
    public DbCredentials SuperUserCredentials { get; }
    
    public PostgresDatabaseFixture()
    {
        var containersConfig = Configuration.Instance.GetRequiredSection("Containers");
        Environment.SetEnvironmentVariable("DOCKER_HOST", containersConfig.GetValue<string>("DockerHost"));
        
        var superUserName = containersConfig["Postgres:SuperUserCredentials:Username"]!;
        var superUserPassword = containersConfig["Postgres:SuperUserCredentials:Password"]!;
        SuperUserCredentials = new DbCredentials(superUserName, superUserPassword);

        _container = new PostgreSqlBuilder()
            .WithImage(containersConfig["Postgres:Image"])
            .WithEnvironment("POSTGRES_USER", superUserName)
            .WithEnvironment("POSTGRES_PASSWORD", superUserPassword)
            .WithDatabase(containersConfig["Postgres:Database"])
            .WithUsername(containersConfig["Postgres:TestUserCredentials:Username"])
            .WithPassword(containersConfig["Postgres:TestUserCredentials:Password"])
            .Build();
        
        _container.StartAsync().Wait();
    }
    
    ~PostgresDatabaseFixture()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
            _container.StopAsync().Wait();
    }
}