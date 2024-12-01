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
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="address">
    /// The database host name or IP address.
    /// </param>
    /// <param name="port">
    /// The database port.
    /// </param>
    public DbHost(DbProvider provider, string address, int? port = null)
    {
        Provider = provider;
        Address = address;
        Port = port ?? Provider.DefaultPort;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHost"/> class.
    /// </summary>
    /// <param name="providerFamily">
    /// The database provider type.
    /// </param>
    /// <param name="address">
    /// The database host name or IP address.
    /// </param>
    /// <param name="port">
    /// The database port.
    /// </param>
    public DbHost(DbProviderFamily providerFamily, string address, int? port) : this(DbProvider.GetInstance(providerFamily), address, port)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHost"/> class.
    /// </summary>
    /// <param name="providerFamily">
    /// The database provider type.
    /// </param>
    /// <param name="connectionStringBuilder">
    /// The connection string builder containing the server name or IP address.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the connection string does not contain a server name or IP address.
    /// </exception>
    public DbHost(DbProviderFamily providerFamily, DbConnectionStringBuilder connectionStringBuilder)
    {
        Provider = DbProvider.GetInstance(providerFamily);
        Address = connectionStringBuilder.GetServer() ?? throw new ArgumentException(Strings.ConnectionStringDoesNotContainAServerNameOrIpAddress);
        Port = connectionStringBuilder.GetPort() ?? Provider.DefaultPort;
    }

    /// <summary>
    /// Gets the database provider.
    /// </summary>
    public DbProvider Provider { get; }
    
    /// <summary>
    /// Gets the database host name or IP address.
    /// </summary>
    public string Address { get; }
    
    /// <summary>
    /// Gets the database port.
    /// </summary>
    public int Port { get; }
}