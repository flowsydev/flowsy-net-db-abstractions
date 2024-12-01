namespace Flowsy.Db.Abstractions;

/// <summary>
/// Contains the commonly used keys for connection string parameters.
/// </summary>
public static class DbConnectionStringKeys
{
    public static readonly string[] Server = [ "Server", "Data Source", "Host" ];
    public static readonly string[] Port = [ "Port" ];
    public static readonly string[] UserName = [ "User ID", "UID", "User", "Username" ];
    public static readonly string[] Password = [ "Password", "PWD" ];
    public static readonly string[] DatabaseName = [ "Database", "Initial Catalog", "AttachDbFilename", "Data Source" ];

    // Optional parameters
    public static readonly string[] IntegratedSecurity = [ "Integrated Security", "Trusted_Connection" ];
    public static readonly string[] ConnectionTimeout = [ "Connection Timeout", "Timeout" ];
    public static readonly string[] Encrypt = [ "Encrypt" ];
    public static readonly string[] TrustServerCertificate = [ "Trust Server Certificate" ];
    public static readonly string[] ApplicationName = [ "Application Name" ];
    public static readonly string[] Pooling = [ "Pooling" ];
    public static readonly string[] MinPoolSize = [ "Min Pool Size", "Minimum Pool Size" ];
    public static readonly string[] MaxPoolSize = [ "Max Pool Size", "Maximum Pool Size" ];
    public static readonly string[] PersistSecurityInfo = [ "Persist Security Info" ];
    public static readonly string[] MultipleActiveResultSets = [ "Multiple Active Result Sets", "MARS" ];
    public static readonly string[] ConnectionLifetime = [ "Connection Lifetime" ];

    // PostgreSQL-specific parameters
    public static readonly string[] SslMode = [ "Ssl Mode" ];
    public static readonly string[] CommandTimeout = [ "Command Timeout", "CommandTimeout" ];
    public static readonly string[] SearchPath = [ "Search Path" ];

    // MySQL-specific parameters
    public static readonly string[] AllowUserVariables = [ "Allow User Variables" ];
    public static readonly string[] UseCompression = [ "Use Compression" ];

    // Oracle-specific parameters
    public static readonly string[] ServiceName = [ "Service Name", "SID" ];

    // SQLite-specific parameters
    public static readonly string[] DataSource = [ "Data Source" ];
    public static readonly string[] Version = [ "Version" ];
    public static readonly string[] Cache = [ "Cache" ];
    public static readonly string[] FailIfMissing = [ "FailIfMissing" ];
    public static readonly string[] ReadOnly = [ "ReadOnly" ];
}