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
    
    /// <summary>
    /// Formats a casting expression for the specified provider.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="expression">
    /// The expression to cast.
    /// </param>
    /// <param name="type">
    /// The type to cast to.
    /// </param>
    /// <returns>
    /// The formatted casting expression.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provider is not supported.
    /// </exception>
    public static string FormatCasting(this DbProvider provider, string expression, string type)
        => provider switch
        {
            DbProvider.PostgreSql => $"{expression}::{type}",
            DbProvider.MySql => $"CAST({expression} AS {type})",
            DbProvider.SqlServer => $"CAST({expression} AS {type})",
            DbProvider.Oracle => $"CAST({expression} AS {type})",
            DbProvider.Sqlite => $"CAST({expression} AS {type})",
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    
    /// <summary>
    /// Gets a value indicating whether the provider supports enums.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <returns>
    /// <c>true</c> if the provider supports enums; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static bool SuportsEnums(this DbProvider provider)
        => provider switch
        {
            DbProvider.PostgreSql => true,
            DbProvider.MySql => true,
            DbProvider.SqlServer => false,
            DbProvider.Oracle => false,
            DbProvider.Sqlite => false,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };

    
    /// <summary>
    /// Gets a value indicating whether the provider supports the specified routine type.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="routineType">
    /// The type of routine.
    /// </param>
    /// <returns>
    /// <c>true</c> if the provider supports the specified routine type; otherwise, <c>false</c>.
    /// </returns>
    public static bool SupportsRoutineType(this DbProvider provider, DbRoutineType routineType)
        => provider != DbProvider.Sqlite || routineType != DbRoutineType.StoredProcedure;

    /// <summary>
    /// Gets a value indicating whether the provider can return a table from a routine of the specified type.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="routineType">
    /// The type of routine.
    /// </param>
    /// <returns>
    /// <c>true</c> if the provider can return a table from a routine of the specified type; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provider is not supported.
    /// </exception>
    public static bool RoutineCanReturnTable(this DbProvider provider, DbRoutineType routineType)
        => routineType == DbRoutineType.StoredProcedure || provider switch
        {
            DbProvider.PostgreSql => true,
            DbProvider.MySql => false,
            DbProvider.SqlServer => true,
            DbProvider.Oracle => true,
            DbProvider.Sqlite => false,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };

    public static bool SupportsNamedParameters(this DbProvider provider)
        => provider is DbProvider.PostgreSql or DbProvider.SqlServer or DbProvider.Oracle;

    /// <summary>
    /// Get the parameter prefix used by the provider when building a SQL statement.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <returns>
    /// The parameter prefix used by the provider when building a SQL statement.
    /// </returns>
    public static string GetParameterPrefixForStatement(this DbProvider provider)
        => provider == DbProvider.Oracle ? ":" : "@";

    public static string FormatNamedParameter(this DbProvider provider, string parameterName, string valueExpression)
    {
        var prefix = provider.GetParameterPrefixForStatement();
        return provider switch
        {
            DbProvider.PostgreSql => $"{parameterName} => {valueExpression}",
            DbProvider.SqlServer => $"{parameterName} = {valueExpression}",
            _ => $"{prefix}{parameterName}",
        };
    }

    /// <summary>
    /// Builds a statement to call the routine.
    /// </summary>
    /// <param name="provider">
    /// The database provider.
    /// </param>
    /// <param name="routine">
    /// The routine to call.
    /// </param>
    /// <returns>
    /// The statement to call the routine.
    /// </returns>
    public static string BuildStatement(this DbProvider provider, DbRoutineDescriptor routine)
    {
        if (routine.ReturnsTable && !provider.RoutineCanReturnTable(routine.Type))
            throw new NotSupportedException(string.Format(Strings.ProviderXCanNotReturnATableFromRoutineOfTypeY, provider, routine.Type));

        var parameterPrefix = provider.GetParameterPrefixForStatement();
        List<string> parameterNames = [];
        List<string> parameterExpressions = [];
        foreach (var parameter in routine.Parameters)
        {
            parameterNames.Add(parameter.Name);
            
            var expression = $"{parameterPrefix}{parameter.Name}";
            if (parameter is {ValueExpression: DbValueExpression.CustomTypeCast, CustomType: not null})
                expression = provider.FormatCasting(expression, parameter.CustomType);
            
            parameterExpressions.Add(expression);
        }
        var parameterListText = routine.UseNamedParameters 
            ? string.Join(", ", parameterExpressions.Select((e, index) => provider.FormatNamedParameter(parameterNames[index], e)))
            : string.Join(", ", parameterExpressions);

        var unsupportedRoutineTypeMessage = string.Format(Strings.ProviderXDoesNotSupportRoutineTypeY, provider, routine.Type);

        return provider switch
        {
            DbProvider.PostgreSql => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"CALL {routine.FullName}({parameterListText})",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? $"SELECT * FROM {routine.FullName}({parameterListText})"
                    : $"SELECT {routine.FullName}({parameterListText})",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProvider.MySql => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"CALL {routine.FullName}({parameterListText})",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? throw new NotSupportedException(unsupportedRoutineTypeMessage)
                    : $"SELECT {routine.FullName}({parameterListText})",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProvider.SqlServer => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"EXEC {routine.FullName} {parameterListText}",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? $"SELECT * FROM {routine.FullName}({parameterListText})"
                    : $"SELECT {routine.FullName}({parameterListText})",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProvider.Oracle => routine.Type switch
            {
                DbRoutineType.StoredProcedure => $"BEGIN {routine.FullName}({parameterListText}); END;",
                DbRoutineType.StoredFunction => routine.ReturnsTable
                    ? $"SELECT * FROM TABLE({routine.FullName}({parameterListText}))"
                    : $"SELECT {routine.FullName}({parameterListText}) FROM DUAL",
                _ => throw new NotSupportedException(unsupportedRoutineTypeMessage)
            },
            DbProvider.Sqlite => routine.Type switch
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
}