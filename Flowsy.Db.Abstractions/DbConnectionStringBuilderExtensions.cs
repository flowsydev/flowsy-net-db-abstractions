using System.Data.Common;

namespace Flowsy.Db.Abstractions;

public static class DbConnectionStringBuilderExtensions
{
    /// <summary>
    /// Gets the server name or IP address from the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <returns>
    /// The server name or IP address.
    /// </returns>
    public static string? GetServer(this DbConnectionStringBuilder connectionStringBuilder)
    {
        string? stringValue = null;
        foreach (var key in DbConnectionStringKeys.Server)
        {
            if (!connectionStringBuilder.TryGetValue(key, out var value)) continue;
            stringValue = value.ToString();
            break;
        }
        return stringValue;
    }
    
    /// <summary>
    /// Gets the port from the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <returns>
    /// The port.
    /// </returns>
    public static int? GetPort(this DbConnectionStringBuilder connectionStringBuilder)
    {
        int? port = null;
        foreach (var key in DbConnectionStringKeys.Port)
        {
            if (!connectionStringBuilder.TryGetValue(key, out var value)) continue;
            port = int.Parse(value.ToString());
            break;
        }
        return port;
    }
    
    /// <summary>
    /// Gets the user name from the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <returns>
    /// The user name.
    /// </returns>
    public static string? GetUserName(this DbConnectionStringBuilder connectionStringBuilder)
    {
        string? stringValue = null;
        foreach (var key in DbConnectionStringKeys.UserName)
        {
            if (connectionStringBuilder.TryGetValue(key, out var value))
            {
                stringValue = value.ToString();
                break;
            }
        }
        return stringValue;
    }
    
    /// <summary>
    /// Sets the user name in the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <param name="userName">
    /// The user name to set.
    /// </param>
    public static void SetUserName(this DbConnectionStringBuilder connectionStringBuilder, string userName)
    {
        var userNameKey = DbConnectionStringKeys.UserName.FirstOrDefault(connectionStringBuilder.ContainsKey) ?? DbConnectionStringKeys.UserName.First();
        connectionStringBuilder[userNameKey] = userName;
    }
    
    /// <summary>
    /// Gets the password from the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <returns></returns>
    public static string? GetPassword(this DbConnectionStringBuilder connectionStringBuilder)
    {
        string? stringValue = null;
        foreach (var key in DbConnectionStringKeys.Password)
        {
            if (!connectionStringBuilder.TryGetValue(key, out var value)) continue;
            stringValue = value.ToString();
            break;
        }
        return stringValue;
    }
    
    /// <summary>
    /// Sets the password in the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <param name="userName">
    /// The password to set.
    /// </param>
    public static void SetPassword(this DbConnectionStringBuilder connectionStringBuilder, string userName)
    {
        var passwordKey = DbConnectionStringKeys.Password.FirstOrDefault(connectionStringBuilder.ContainsKey) ?? DbConnectionStringKeys.Password.First();
        connectionStringBuilder[passwordKey] = userName;
    }
    
    /// <summary>
    /// Gets the database name from the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <returns>
    /// The database name.
    /// </returns>
    public static string? GetDatabaseName(this DbConnectionStringBuilder connectionStringBuilder)
    {
        string? stringValue = null;
        foreach (var key in DbConnectionStringKeys.DatabaseName)
        {
            if (!connectionStringBuilder.TryGetValue(key, out var value)) continue;
            stringValue = value.ToString();
            break;
        }
        return stringValue;
    }

    /// <summary>
    /// Sets the database name in the connection string.
    /// </summary>
    /// <param name="connectionStringBuilder">
    /// The connection string builder.
    /// </param>
    /// <param name="databaseName"></param>
    public static void SetDatabaseName(this DbConnectionStringBuilder connectionStringBuilder, string databaseName)
    {
        var databaseNameKey = DbConnectionStringKeys.DatabaseName.FirstOrDefault(connectionStringBuilder.ContainsKey) ?? DbConnectionStringKeys.DatabaseName.First();
        connectionStringBuilder[databaseNameKey] = databaseName;
    }
}