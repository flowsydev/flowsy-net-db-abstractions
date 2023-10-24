using System.Data;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a mechanism to obtain database connections.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// A list of keys known by the connection factory.
    /// Each key identifies a database connection that can be obtained by the connection factory.
    /// </summary>
    IEnumerable<string> ConnectionKeys { get; }
    
    /// <summary>
    /// Obtains a database connection.
    /// </summary>
    /// <param name="key">The connection identifier.</param>
    /// <returns>A database connection</returns>
    IDbConnection GetConnection(string? key = null);
}