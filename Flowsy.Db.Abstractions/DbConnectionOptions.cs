using System.Data;
using System.Data.Common;
using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents the options for a database connection.
/// </summary>
public sealed class DbConnectionOptions
{
    private readonly DbConnectionStringBuilder _connectionStringBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="provider">
    /// The provider to use for the connection.
    /// </param>
    /// <param name="host">
    /// The host name or IP address of the database server.
    /// </param>
    /// <param name="credentials">
    /// The credentials to use for the connection.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database to connect to.
    /// </param>
    public DbConnectionOptions(DbProvider provider, DbHost host, DbCredentials credentials, string? databaseName = null)
    {
        Provider = provider;
        Host = host;
        _connectionStringBuilder = host.CreateConnectionStringBuilder(Provider, credentials, databaseName);
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder to use for the connection.
    /// </param>
    /// /// <param name="provider">
    /// The provider to use for the connection.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string builder does not contain a server name or IP address.
    /// </exception>
    public DbConnectionOptions(DbConnectionStringBuilder connectionStringBuilder, DbProvider provider)
    {
        var address = connectionStringBuilder.GetServer() ?? throw new ArgumentException(Strings.ConnectionStringDoesNotContainAServerNameOrIpAddress, nameof(connectionStringBuilder));
        _connectionStringBuilder = connectionStringBuilder;
        Provider = provider;
        Host = new DbHost(address, _connectionStringBuilder.GetPort() ?? Provider.DefaultPort);
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder to use for the connection.
    /// </param>
    /// <param name="providerFamily">
    /// The provider to use for the connection.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string builder does not contain a server name or IP address.
    /// </exception>
    public DbConnectionOptions(DbConnectionStringBuilder connectionStringBuilder, DbProviderFamily providerFamily)
        : this(connectionStringBuilder.ConnectionString, providerFamily)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder to use for the connection.
    /// </param>
    /// <param name="providerInvariantName">
    /// The invariant name of the provider to use for the connection.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string builder does not contain a server name or IP address.
    /// </exception>
    public DbConnectionOptions(DbConnectionStringBuilder connectionStringBuilder, string providerInvariantName)
        : this(connectionStringBuilder, DbProvider.GetInstance(providerInvariantName))
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="connectionString">
    /// The connection string to use for the connection.
    /// </param>
    /// <param name="provider">
    /// The provider to use for the connection.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string does not contain a server name or IP address.
    /// </exception>
    public DbConnectionOptions(string connectionString, DbProvider provider) : this(new DbConnectionStringBuilder { ConnectionString = connectionString }, provider)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="connectionString">
    /// The connection string to use for the connection.
    /// </param>
    /// <param name="providerFamily">
    /// The provider to use for the connection.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string does not contain a server name or IP address.
    /// </exception> 
    public DbConnectionOptions(string connectionString, DbProviderFamily providerFamily) : this(connectionString, DbProvider.GetInstance(providerFamily))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="connectionString">
    /// The connection string to use for the connection.
    /// </param>
    /// <param name="providerInvariantName">
    /// The invariant name of the provider to use for the connection.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string does not contain a server name or IP address.
    /// </exception>
    public DbConnectionOptions(string connectionString, string providerInvariantName)
        : this(connectionString, DbProvider.GetInstance(providerInvariantName))
    {
    }
    
    /// <summary>
    /// Gets the provider associated with the connection options.
    /// </summary>
    public DbProvider Provider { get; }
    
    /// <summary>
    /// Gets the host of the database associated with the connection options.
    /// </summary>
    public DbHost Host { get; }
    
    /// <summary>
    /// Gets the connection string associated with the connection options.
    /// </summary>
    public string ConnectionString => _connectionStringBuilder.ConnectionString;
    
    /// <summary>
    /// Gets the name of the database associated with the connection options.
    /// </summary>
    public string DatabaseName => _connectionStringBuilder.GetDatabaseName() ?? string.Empty;
    
    /// <summary>
    /// Gets a new connection to the database.
    /// </summary>
    /// <param name="userName">
    /// The user name to use for the connection.
    /// </param>
    /// <param name="password">
    /// The password to use for the connection.
    /// </param>
    /// <param name="defaultDatabase">
    /// Whether to use the default database for underlying provider.
    /// </param>
    /// <param name="open">
    /// Whether to open the connection after creating it.
    /// </param>
    /// <returns>
    /// A new connection to the database.
    /// </returns>
    public IDbConnection GetConnection(
        string? userName = null,
        string? password = null,
        bool defaultDatabase = false,
        bool open = false
    )
    {
        var connection =
            Provider.Factory.CreateConnection() ?? 
            throw new InvalidOperationException(string.Format(Strings.FailedToCreateConnectionForProviderX, Provider.Family));
        
        var builder = new DbConnectionStringBuilder()
        {
            ConnectionString = ConnectionString
        };
        
        if (!string.IsNullOrEmpty(userName))
            builder.SetUserName(userName);
        
        if (!string.IsNullOrEmpty(password))
            builder.SetPassword(password);

        if (defaultDatabase && Provider.Family != DbProviderFamily.Oracle && Provider.Family != DbProviderFamily.Sqlite)
        {
            var defaultDatabaseName = Provider.DefaultDatabaseName;
            if (!string.IsNullOrEmpty(defaultDatabaseName))
                builder.SetDatabaseName(defaultDatabaseName);
        }
        
        connection.ConnectionString = builder.ConnectionString;
        
        if (open)
            connection.Open();
        
        return connection;
    }

    /// <summary>
    /// Returns a string representation of the connection options.
    /// </summary>
    /// <returns>
    /// A string representation of the connection options.
    /// </returns>
    public override string ToString()
    {
        var csb = new DbConnectionStringBuilder
        {
            ConnectionString = ConnectionString
        };
        csb.SetPassword("********");

        return $"[{Provider.Family}] {csb.ConnectionString}";
    }
}