using System.Data.Common;
using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a database host.
/// </summary>
public sealed class DbHost
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbHost"/> class.
    /// </summary>
    /// <param name="address">
    /// The host name or IP address of the database server.
    /// </param>
    /// <param name="port">
    /// The port number of the database server.
    /// </param>
    public DbHost(string address, int port)
    {
        Address = address;
        Port = port;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHost"/> class.
    /// </summary>
    /// <param name="connectionString">
    /// The connection string containing the server name or IP address and port.
    /// </param>
    /// <param name="providerFamily">
    /// The database provider type.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string does not contain a server name or IP address.
    /// </exception>
    public DbHost(string connectionString, DbProviderFamily providerFamily) : this (new DbConnectionStringBuilder { ConnectionString = connectionString}, providerFamily)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHost"/> class.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder containing the server name or IP address.
    /// </param>
    /// <param name="providerFamily">
    /// The database provider type.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string builder does not contain a server name or IP address.
    /// </exception>
    public DbHost(DbConnectionStringBuilder connectionStringBuilder, DbProviderFamily providerFamily)
    {
        // Provider = DbProvider.GetInstance(providerFamily);
        Address = connectionStringBuilder.GetServer() ?? throw new ArgumentException(Strings.ConnectionStringDoesNotContainAServerNameOrIpAddress, nameof(connectionStringBuilder));
        Port = connectionStringBuilder.GetPort() ?? DbProvider.GetInstance(providerFamily).DefaultPort;
    }

    /// <summary>
    /// Gets the database host name or IP address.
    /// </summary>
    public string Address { get; }
    
    /// <summary>
    /// Gets the database port.
    /// </summary>
    public int Port { get; }
    
    /// <summary>
    /// Creates a connection string builder for the host.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="credentials">
    /// The database credentials.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database to connect to.
    /// </param>
    /// <param name="additionalParameters">
    /// Additional connection parameters.
    /// </param>
    /// <returns>
    /// The connection string builder.
    /// </returns>
    public DbConnectionStringBuilder CreateConnectionStringBuilder(DbProvider provider, DbCredentials credentials, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
        => provider.CreateConnectionStringBuilder(this, credentials, databaseName, additionalParameters);
    
    /// <summary>
    /// Creates a connection string builder for the host.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="userName">
    /// The user name to use for the connection.
    /// </param>
    /// <param name="password">
    /// The password to use for the connection.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database to connect to.
    /// </param>
    /// <param name="additionalParameters">
    /// Additional connection parameters.
    /// </param>
    /// <returns>
    /// The connection string builder.
    /// </returns>
    public DbConnectionStringBuilder CreateConnectionStringBuilder(
        DbProvider provider,
        string userName, 
        string password,
        string? databaseName = null,
        IDictionary<string, string>? additionalParameters = null
        )
        => provider.CreateConnectionStringBuilder(Address, Port, userName, password, databaseName, additionalParameters);

    /// <summary>
    /// Builds a connection string for the host.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="credentials">
    /// The credentials to use for the connection.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database to connect to.
    /// </param>
    /// <param name="additionalParameters">
    /// Additional parameters to include in the connection string.
    /// </param>
    /// <returns>
    /// The connection string.
    /// </returns>
    public string BuildConnectionString(DbProvider provider, DbCredentials credentials, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
        => provider.BuildConnectionString(this, credentials, databaseName, additionalParameters);

    /// <summary>
    /// Builds a connection string for the host.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="username">
    /// The user name to use for the connection.
    /// </param>
    /// <param name="password">
    /// The password to use for the connection.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database to connect to.
    /// </param>
    /// <param name="additionalParameters">
    /// Additional parameters to include in the connection string.
    /// </param>
    /// <returns>
    /// The connection string.
    /// </returns>
    public string BuildConnectionString(DbProvider provider, string username, string password, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
        => provider.BuildConnectionString(Address, Port, username, password, databaseName, additionalParameters);

    /// <summary>
    /// Creates connection options for the host.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="credentials">
    /// The credentials to use for the connection.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database to connect to.
    /// </param>
    /// <param name="additionalParameters">
    /// Additional parameters to include in the connection string.
    /// </param>
    /// <returns>
    /// The connection options.
    /// </returns>
    public DbConnectionOptions CreateConnectionOptions(DbProvider provider, DbCredentials credentials, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
        => new (provider, BuildConnectionString(provider, credentials, databaseName, additionalParameters));
}