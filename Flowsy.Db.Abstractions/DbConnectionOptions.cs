using System.Data;
using System.Data.Common;
using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents the options for a database connection.
/// </summary>
public sealed class DbConnectionOptions
{
    private static readonly string[] UserNameKeys = ["User ID", "UID", "User", "Username"];
    private static readonly string[] PasswordKeys = ["Password", "PWD"];
    private static readonly string[] DatabaseNameKeys = ["Database", "Initial Catalog", "AttachDbFilename"];
    
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
        Provider = host.Provider;
        
        var connectionStringBuilder = new DbConnectionStringBuilder
        {
            {"Server", host.Address},
            {"Port", host.Port},
            {"User ID", credentials.UserName},
            {"Password", credentials.Password}
        };

        if (!string.IsNullOrEmpty(databaseName))
            connectionStringBuilder.Add("Database", databaseName);
        
        ConnectionString = connectionStringBuilder.ConnectionString;
        DatabaseName = ResolveDatabaseName(ConnectionString);
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
    public DbConnectionOptions(DbProvider provider, string connectionString)
    {
        Provider = provider;
        ConnectionString = connectionString;
        DatabaseName = ResolveDatabaseName(connectionString);
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
    {
        Provider = providerInvariantName switch
        {
            "Npgsql" => DbProvider.PostgreSql,
            "MySql.Data" => DbProvider.MySql,
            "System.Data.SqlClient" => DbProvider.SqlServer,
            "Oracle.ManagedDataAccess.Client" => DbProvider.Oracle,
            "Microsoft.Data.Sqlite" => DbProvider.Sqlite,
            _ => throw new NotSupportedException(string.Format(Strings.ProviderXIsNotSupported, providerInvariantName))
        };
        
        ConnectionString = connectionString;
        DatabaseName = ResolveDatabaseName(connectionString);
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
    public DbConnectionOptions(DbProvider provider, DbConnectionStringBuilder connectionStringBuilder)
        : this(provider, connectionStringBuilder.ConnectionString)
    {
    }

    private static string ResolveDatabaseName(string connectionString)
    {
        var builder = new DbConnectionStringBuilder()
        {
            ConnectionString = connectionString
        };
        
        foreach (var key in DatabaseNameKeys)
        {
            if (builder.TryGetValue(key, out var databaseName))
                return databaseName!.ToString()!;
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets the provider associated with the connection options.
    /// </summary>
    public DbProvider Provider { get; }
    
    /// <summary>
    /// Gets the connection string associated with the connection options.
    /// </summary>
    public string ConnectionString { get; }
    
    /// <summary>
    /// Gets the name of the database associated with the connection options.
    /// </summary>
    public string DatabaseName { get; }
    
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
        bool open = true
    )
    {
        var providerName = Provider.GetInvariantName();
        var connection =
            DbProviderFactories.GetFactory(providerName).CreateConnection() ?? 
            throw new InvalidOperationException(string.Format(Strings.FailedToCreateConnectionForProviderX, providerName));
        
        var builder = new DbConnectionStringBuilder()
        {
            ConnectionString = ConnectionString
        };

        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            var userNameKey = UserNameKeys.FirstOrDefault(key => builder.ContainsKey(key)) ?? UserNameKeys.First();
            builder[userNameKey] = userName;
            
            var passwordKey = PasswordKeys.FirstOrDefault(key => builder.ContainsKey(key)) ?? PasswordKeys.First();
            builder[passwordKey] = password;
        }
        
        if (defaultDatabase && Provider != DbProvider.Oracle && Provider != DbProvider.Sqlite)
        {
            var defaultDatabaseName = Provider.GetDefaultDatabaseName();
            foreach (var key in DatabaseNameKeys)
            {
                if (builder.ContainsKey(key))
                    builder[key] = defaultDatabaseName;
            }
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
        foreach (var key in PasswordKeys)
            if (csb.ContainsKey(key)) csb[key] = "********";
        
        List<string> values =
        [
            $"Provider: {Provider}",
            $"ConnectionString: {csb.ConnectionString}",
        ];
        
        return string.Join(" | ", values);
    }
}