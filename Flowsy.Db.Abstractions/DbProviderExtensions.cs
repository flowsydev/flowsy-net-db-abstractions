using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Extension methods for <see cref="DbProvider"/>.
/// </summary>
public static class DbProviderExtensions
{
    /// <summary>
    /// Get the invariant name of the provider.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <returns>
    /// The invariant name of the provider.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provider is not supported.
    /// </exception>
    public static string GetInvariantName(this DbProvider provider)
        => provider switch
        {
            DbProvider.PostgreSql => "Npgsql",
            DbProvider.MySql => "MySql.Data",
            DbProvider.SqlServer => "System.Data.SqlClient",
            DbProvider.Oracle => "Oracle.ManagedDataAccess.Client",
            DbProvider.Sqlite => "Microsoft.Data.Sqlite",
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    
    /// <summary>
    /// Get the default port used by the provider.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <returns>
    /// The default port used by the provider.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provider is not supported.
    /// </exception>
    public static int GetDefaultPort(this DbProvider provider)
        => provider switch
        {
            DbProvider.PostgreSql => 5432,
            DbProvider.MySql => 3306,
            DbProvider.SqlServer => 1433,
            DbProvider.Oracle => 1521,
            DbProvider.Sqlite => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    
    /// <summary>
    /// Get the default database name used by the provider.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <returns>
    /// The default database name used by the provider.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the provider does not have a default database name.
    /// </exception>
    public static string GetDefaultDatabaseName(this DbProvider provider)
        => provider switch
        {
            DbProvider.PostgreSql => "postgres",
            DbProvider.MySql => "information_schema",
            DbProvider.SqlServer => "master",
            _ => throw new NotSupportedException(string.Format(Strings.ProviderXDoesNotHaveADefaultDatabaseName, provider))
        };
}