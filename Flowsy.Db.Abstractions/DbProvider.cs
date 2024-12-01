using System.Data.Common;
using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a database provider.
/// </summary>
public class DbProvider
{
    private static readonly Dictionary<DbProviderFamily, DbProvider> Providers = new ();

    /// <summary>
    /// Registers a provider with the specified invariant name, provider type and factory.
    /// </summary>
    /// <param name="invariantName">
    /// The invariant name of the provider.
    /// </param>
    /// <param name="family">
    /// The provider family.
    /// </param>
    /// <param name="factory">
    /// The factory for the provider.
    /// </param>
    /// <returns>
    /// The registered provider.
    /// </returns>
    public static DbProvider Register(string invariantName, DbProviderFamily family, DbProviderFactory factory)
    {
        DbProviderFactories.RegisterFactory(invariantName, factory);
        var provider = new DbProvider(invariantName, family);
        Providers[family] = provider;
        return provider;
    }
    
    /// <summary>
    /// Gets the provider instance for the specified provider type.
    /// </summary>
    /// <param name="family">
    /// The provider family.
    /// </param>
    /// <returns>
    /// The provider instance for the specified provider type.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the provider type is not supported.
    /// </exception>
    public static DbProvider GetInstance(DbProviderFamily family)
        => Providers.TryGetValue(family, out var provider)
            ? provider
            : throw new NotSupportedException(string.Format(Strings.ProviderXIsNotSupported, family));

    /// <summary>
    /// Gets the provider instance for the specified provider invariant name.
    /// </summary>
    /// <param name="invariantName">
    /// The invariant name of the provider.
    /// </param>
    /// <returns>
    /// The provider instance for the specified provider invariant name.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the provider invariant name is not supported.
    /// </exception>
    public static DbProvider GetInstance(string invariantName)
        => Providers.Values.ToArray().FirstOrDefault(p => p.InvariantName == invariantName) ??
           throw new NotSupportedException(string.Format(Strings.ProviderXIsNotSupported, invariantName));

    /// <summary>
    /// Initializes a new instance of the <see cref="DbProvider"/> class.
    /// </summary>
    /// <param name="invariantName">
    /// The invariant name of the provider.
    /// </param>
    /// <param name="family">
    /// The provider family.
    /// </param>
    public DbProvider(string invariantName, DbProviderFamily family)
    {
        InvariantName = invariantName;
        Family = family;
        switch (Family)
        {
            case DbProviderFamily.PostgreSql:
                DefaultPort = 5432;
                DefaultDatabaseName = "postgres";
                DefaultSchemaName = "public";
                break;
            case DbProviderFamily.MySql:
                DefaultPort = 3306;
                DefaultDatabaseName = "mysql";
                DefaultSchemaName = "mysql";
                break;
            case DbProviderFamily.SqlServer:
                DefaultPort = 1433;
                DefaultDatabaseName = "master";
                DefaultSchemaName = "dbo";
                break;
            case DbProviderFamily.Oracle:
                DefaultPort = 1521;
                DefaultDatabaseName = null;
                DefaultSchemaName = null;
                break;
            case DbProviderFamily.Sqlite:
                DefaultPort = 0;
                DefaultDatabaseName = null;
                DefaultSchemaName = null;
                break;
            default:
                DefaultPort = 0;
                DefaultDatabaseName = null;
                DefaultSchemaName = null;
                break;
        }
    }

    /// <summary>
    /// Gets the provider family.
    /// </summary>
    public DbProviderFamily Family { get; }
    
    /// <summary>
    /// Gets the invariant name for the provider.
    /// </summary>
    public string InvariantName { get; }
    
    /// <summary>
    /// Gets the default port for the provider.
    /// </summary>
    public int DefaultPort { get; }
    
    /// <summary>
    /// Gets the default database name for the provider.
    /// </summary>
    public string? DefaultDatabaseName { get; }
    
    /// <summary>
    /// Gets the default schema for the provider.
    /// </summary>
    public string? DefaultSchemaName { get; }
    
    /// <summary>
    /// Gets the factory for the provider.
    /// </summary>
    /// <returns>
    /// The factory for the provider.
    /// </returns>
    public DbProviderFactory Factory
        => DbProviderFactories.GetFactory(InvariantName);
    
    /// <summary>
    /// Gets the parameter prefix for a statement.
    /// </summary>
    public string ParameterPrefixForStatement
        => Family == DbProviderFamily.Oracle ? ":" : "@";
    
    /// <summary>
    /// Gets a value indicating whether the provider supports named parameters.
    /// </summary>
    public bool SupportsNamedParameters 
        => Family is DbProviderFamily.PostgreSql or DbProviderFamily.SqlServer or DbProviderFamily.Oracle;
    
    /// <summary>
    /// Gets a value indicating whether the provider supports enums.
    /// </summary>
    public bool SupportsEnums
        => Family switch
        {
            DbProviderFamily.PostgreSql => true,
            DbProviderFamily.MySql => true,
            DbProviderFamily.SqlServer => false,
            DbProviderFamily.Oracle => false,
            DbProviderFamily.Sqlite => false,
            _ => false
        };
    
    /// <summary>
    /// Gets a value indicating whether the provider supports the specified routine type.
    /// </summary>
    /// <param name="routineType">
    /// The type of routine.
    /// </param>
    /// <returns>
    /// <c>true</c> if the provider supports the specified routine type; otherwise, <c>false</c>.
    /// </returns>
    public bool SupportsRoutineType(DbRoutineType routineType)
        => Family != DbProviderFamily.Sqlite || routineType != DbRoutineType.StoredProcedure;
    
    /// <summary>
    /// Gets a value indicating whether the provider can return a table from a routine of the specified type.
    /// </summary>
    /// <param name="routineType">
    /// The type of routine.
    /// </param>
    /// <returns>
    /// <c>true</c> if the provider can return a table from a routine of the specified type; otherwise, <c>false</c>.
    /// </returns>
    public bool RoutineCanReturnTable(DbRoutineType routineType)
        => routineType == DbRoutineType.StoredProcedure || Family switch
        {
            DbProviderFamily.PostgreSql => true,
            DbProviderFamily.MySql => false,
            DbProviderFamily.SqlServer => true,
            DbProviderFamily.Oracle => true,
            DbProviderFamily.Sqlite => false,
            _ => false
        };
    
    /// <summary>
    /// Formats a casting expression for this provider.
    /// </summary>
    /// <param name="expression">
    /// The expression to cast.
    /// </param>
    /// <param name="type">
    /// The type to cast to.
    /// </param>
    /// <returns>
    /// The formatted casting expression.
    /// </returns>
    public string FormatCasting(string expression, string type)
        => Family switch
        {
            DbProviderFamily.PostgreSql => $"{expression}::{type}",
            _ => $"CAST({expression} AS {type})"
        };
    
    /// <summary>
    /// Formats a named parameter for this provider.
    /// </summary>
    /// <param name="parameterName">
    /// The name of the parameter.
    /// </param>
    /// <param name="valueExpression">
    /// The expression for the value.
    /// </param>
    /// <returns>
    /// The formatted named parameter.
    /// </returns>
    public string FormatNamedParameter(string parameterName, string valueExpression)
        => Family switch
        {
            DbProviderFamily.PostgreSql => $"{parameterName} => {valueExpression}",
            DbProviderFamily.SqlServer => $"{parameterName} = {valueExpression}",
            _ => $"{ParameterPrefixForStatement}{parameterName}",
        };
    
    /// <summary>
    /// Gets a connection string builder using the specified host and credentials.
    /// </summary>
    /// <param name="host">
    /// The host of the database.
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
    /// The connection string builder.
    /// </returns>
    public DbConnectionStringBuilder GetConnectionStringBuilder(DbHost host, DbCredentials credentials, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
        => GetConnectionStringBuilder(host.Address, host.Port, credentials.UserName, credentials.Password, databaseName, additionalParameters);

    /// <summary>
    /// Gets a connection string builder using the specified host, port and credentials.
    /// </summary>
    /// <param name="host">
    /// The host of the database.
    /// </param>
    /// <param name="port">
    /// The port of the database.
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
    /// The connection string builder.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the provider does not support connection string building.
    /// </exception>
    public DbConnectionStringBuilder GetConnectionStringBuilder(string host, int port, string username, string password, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
    {
        var connectionStringBuilder = Factory.CreateConnectionStringBuilder();
        if (connectionStringBuilder is null)
            throw new NotSupportedException(string.Format(Strings.ProviderXDoesNotSupportConnectionStringBuilding, Family));

        // Add parameters based on the provider type
        switch (Family)
        {
            case DbProviderFamily.PostgreSql:
                connectionStringBuilder.Add("Host", host);
                connectionStringBuilder.Add("Port", port);
                connectionStringBuilder.Add("Username", username);
                connectionStringBuilder.Add("Password", password);
                if (!string.IsNullOrEmpty(databaseName))
                    connectionStringBuilder.Add("Database", databaseName);
                break;
            case DbProviderFamily.MySql:
                connectionStringBuilder.Add("Server", host);
                connectionStringBuilder.Add("Port", port);
                connectionStringBuilder.Add("User Id", username);
                connectionStringBuilder.Add("Password", password);
                if (!string.IsNullOrEmpty(databaseName))
                    connectionStringBuilder.Add("Database", databaseName);
                break;
            case DbProviderFamily.SqlServer:
                connectionStringBuilder.Add("Data Source", $"{host},{port}");
                connectionStringBuilder.Add("User Id", username);
                connectionStringBuilder.Add("Password", password);
                if (!string.IsNullOrEmpty(databaseName))
                    connectionStringBuilder.Add("Initial Catalog", databaseName);
                break;
            case DbProviderFamily.Oracle:
                var description = $"DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))";
                var connectData = string.Empty;
                if (!string.IsNullOrEmpty(databaseName))
                    connectData += $"(CONNECT_DATA=(SERVICE_NAME={databaseName}))";
                
                connectionStringBuilder.Add("Data Source", $"({description} ){connectData}");
                connectionStringBuilder.Add("User Id", username);
                connectionStringBuilder.Add("Password", password);
                break;
            case DbProviderFamily.Sqlite:
                connectionStringBuilder.Add("Data Source", host);
                break;
            default:
                throw new NotSupportedException(string.Format(Strings.ProviderXDoesNotSupportConnectionStringBuilding, Family));
        }

        if (additionalParameters is null || additionalParameters.Count == 0)
            return connectionStringBuilder;
        
        foreach (var (key, value) in additionalParameters)
            connectionStringBuilder.Add(key, value);

        return connectionStringBuilder;
    }

    /// <summary>
    /// Builds a connection string using the specified host and credentials.
    /// </summary>
    /// <param name="host">
    /// The host of the database.
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
    public string BuildConnectionString(DbHost host, DbCredentials credentials, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
        => BuildConnectionString(host.Address, host.Port, credentials.UserName, credentials.Password, databaseName, additionalParameters);
    
    /// <summary>
    /// Builds a connection string using the specified host, port and credentials.
    /// </summary>
    /// <param name="host">
    /// The host of the database.
    /// </param>
    /// <param name="port">
    /// The port of the database.
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
    public string BuildConnectionString(string host, int port, string username, string password, string? databaseName = null, IDictionary<string, string>? additionalParameters = null)
    {
        var connectionStringBuilder = GetConnectionStringBuilder(host, port, username, password, databaseName, additionalParameters);
        return connectionStringBuilder.ConnectionString;
    }
    
    /// <summary>
    /// Builds a statement to call the given routine.
    /// </summary>
    /// <param name="routine">
    /// The routine to call.
    /// </param>
    /// <returns>
    /// The statement to call the routine.
    /// </returns>
    public string BuildStatement(DbRoutineDescriptor routine)
    {
        if (routine.ReturnsTable && !RoutineCanReturnTable(routine.Type))
            throw new NotSupportedException(string.Format(Strings.ProviderXCanNotReturnATableFromRoutineOfTypeY, Family, routine.Type));

        var parameterPrefix = ParameterPrefixForStatement;
        List<string> parameterNames = [];
        List<string> parameterExpressions = [];
        foreach (var parameter in routine.Parameters)
        {
            parameterNames.Add(parameter.Name);
            
            var expression = $"{parameterPrefix}{parameter.Name}";
            if (parameter is {ValueExpression: DbValueExpression.CustomTypeCast, CustomType: not null})
                expression = FormatCasting(expression, parameter.CustomType);
            
            parameterExpressions.Add(expression);
        }
        var parameterListText = routine.UseNamedParameters 
            ? string.Join(", ", parameterExpressions.Select((e, index) => FormatNamedParameter(parameterNames[index], e)))
            : string.Join(", ", parameterExpressions);

        var unsupportedRoutineTypeMessage = string.Format(Strings.ProviderXDoesNotSupportRoutineTypeY, Family, routine.Type);

        return Family switch
        {
            DbProviderFamily.PostgreSql => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"CALL {routine.FullName}({parameterListText})",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? $"SELECT * FROM {routine.FullName}({parameterListText})"
                    : $"SELECT {routine.FullName}({parameterListText})",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProviderFamily.MySql => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"CALL {routine.FullName}({parameterListText})",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? throw new NotSupportedException(unsupportedRoutineTypeMessage)
                    : $"SELECT {routine.FullName}({parameterListText})",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProviderFamily.SqlServer => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"EXEC {routine.FullName} {parameterListText}",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? $"SELECT * FROM {routine.FullName}({parameterListText})"
                    : $"SELECT {routine.FullName}({parameterListText})",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProviderFamily.Oracle => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"BEGIN {routine.FullName}({parameterListText}); END;",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? $"SELECT * FROM TABLE({routine.FullName}({parameterListText}))"
                    : $"SELECT {routine.FullName}({parameterListText}) FROM DUAL",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProviderFamily.Sqlite => routine.Type switch
            {
                DbRoutineType.StoredProcedure => throw new NotSupportedException(unsupportedRoutineTypeMessage),
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? throw new NotSupportedException(unsupportedRoutineTypeMessage)
                    : $"SELECT {routine.FullName}({parameterListText})",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
        };
    }

    public override string ToString() => $"{Family} ({InvariantName})";
}