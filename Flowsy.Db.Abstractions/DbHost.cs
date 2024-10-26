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
        Port = port ?? provider.GetDefaultPort();
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