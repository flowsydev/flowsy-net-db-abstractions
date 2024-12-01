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
    /// <param name="host">
    /// The host name or IP address of the database server.
    /// </param>
    /// <param name="credentials">
    /// The credentials to use for the connection.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database to connect to.
    /// </param>
    public DbConnectionOptions(DbHost host, DbCredentials credentials, string? databaseName = null)
    {
        Host = host;
        _connectionStringBuilder = host.Provider.GetConnectionStringBuilder(host, credentials, databaseName);
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="provider">
    /// The provider to use for the connection.
    /// </param>
    /// <param name="connectionString">
    /// The connection string to use for the connection.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the connection string does not contain a server name or IP address.
    /// </exception>
    public DbConnectionOptions(DbProvider provider, string connectionString)
    {
        _connectionStringBuilder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString
        };
        var address = _connectionStringBuilder.GetServer() ?? throw new InvalidOperationException(Strings.ConnectionStringDoesNotContainAServerNameOrIpAddress);
        Host = new DbHost(provider, address, _connectionStringBuilder.GetPort());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="provider">
    /// The provider to use for the connection.
    /// </param>
    /// <param name="connectionStringBuilder">
    /// The connection string builder to use for the connection.
    /// </param>
    public DbConnectionOptions(DbProvider provider, DbConnectionStringBuilder connectionStringBuilder) : this(provider, connectionStringBuilder.ConnectionString)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="providerFamily">
    /// The provider to use for the connection.
    /// </param>
    /// <param name="connectionString">
    /// The connection string to use for the connection.
    /// </param>
    public DbConnectionOptions(DbProviderFamily providerFamily, string connectionString) : this(DbProvider.GetInstance(providerFamily), connectionString)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="providerFamily">
    /// The provider to use for the connection.
    /// </param>
    /// <param name="connectionStringBuilder">
    /// The connection string builder to use for the connection.
    /// </param>
    public DbConnectionOptions(DbProviderFamily providerFamily, DbConnectionStringBuilder connectionStringBuilder)
        : this(providerFamily, connectionStringBuilder.ConnectionString)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="providerInvariantName">
    /// The invariant name of the provider to use for the connection.
    /// </param>
    /// <param name="connectionString">
    /// The connection string to use for the connection.
    /// </param>
    public DbConnectionOptions(string providerInvariantName, string connectionString)
        : this(DbProvider.GetInstance(providerInvariantName), connectionString)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionOptions"/> class.
    /// </summary>
    /// <param name="providerInvariantName">
    /// The invariant name of the provider to use for the connection.
    /// </param>
    /// <param name="connectionStringBuilder">
    /// The connection string builder to use for the connection.
    /// </param>
    public DbConnectionOptions(string providerInvariantName, DbConnectionStringBuilder connectionStringBuilder)
        : this(DbProvider.GetInstance(providerInvariantName), connectionStringBuilder)
    {
    }
    
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
    /// Whether to use the default database for the connection.
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
            Host.Provider.Factory.CreateConnection() ?? 
            throw new InvalidOperationException(string.Format(Strings.FailedToCreateConnectionForProviderX, Host.Provider.Family));
        
        var builder = new DbConnectionStringBuilder()
        {
            ConnectionString = ConnectionString
        };
        
        if (!string.IsNullOrEmpty(userName))
            builder.SetUserName(userName);
        
        if (!string.IsNullOrEmpty(password))
            builder.SetPassword(password);

        if (defaultDatabase && Host.Provider.Family != DbProviderFamily.Oracle && Host.Provider.Family != DbProviderFamily.Sqlite)
        {
            var defaultDatabaseName = Host.Provider.DefaultDatabaseName;
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

        return $"[{Host.Provider.Family}] {csb.ConnectionString}";
    }
}