namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a database routine.
/// </summary>
public class DbRoutineDescriptor
{
    /// <summary>
    /// Constructs a new <see cref="DbRoutineDescriptor"/>.
    /// </summary>
    /// <param name="schemaName">
    /// The name of the schema where the routine is located.
    /// </param>
    /// <param name="routineName">
    /// The name of the routine.
    /// </param>
    /// <param name="type">
    /// The type of the routine.
    /// </param>
    /// <param name="returnsTable">
    /// Whether the routine returns a table.
    /// </param>
    /// <param name="parameters">
    /// The parameters of the routine.
    /// </param>
    public DbRoutineDescriptor(string? schemaName, string routineName, DbRoutineType type, bool returnsTable, params DbParameterDescriptor[] parameters)
        : this(schemaName, routineName, type, returnsTable, false, parameters)
    {
    }
    
    /// <summary>
    /// Constructs a new <see cref="DbRoutineDescriptor"/>.
    /// </summary>
    /// <param name="schemaName">
    /// The name of the schema where the routine is located.
    /// </param>
    /// <param name="routineName">
    /// The name of the routine.
    /// </param>
    /// <param name="type">
    /// The type of the routine.
    /// </param>
    /// <param name="returnsTable">
    /// Whether the routine returns a table.
    /// </param>
    /// <param name="useNamedParameters">
    /// Whether to use named parameters.
    /// </param>
    /// <param name="parameters">
    /// The parameters of the routine.
    /// </param>
    public DbRoutineDescriptor(string? schemaName, string routineName, DbRoutineType type, bool returnsTable, bool useNamedParameters, params DbParameterDescriptor[] parameters)
    {
        SchemaName = schemaName;
        RoutineName = routineName;
        Type = type;
        ReturnsTable = returnsTable;
        UseNamedParameters = useNamedParameters;
        Parameters = parameters;
    }

    /// <summary>
    /// The name of the schema where the routine is located.
    /// </summary>
    public string? SchemaName { get; }
    
    /// <summary>
    /// The name of the routine.
    /// </summary>
    public string RoutineName { get; }
    
    /// <summary>
    /// The full name of the routine.
    /// </summary>
    public string FullName => SchemaName == null ? RoutineName : $"{SchemaName}.{RoutineName}";
    
    /// <summary>
    /// The type of the routine.
    /// </summary>
    public DbRoutineType Type { get; }
    
    /// <summary>
    /// Whether the routine returns a table.
    /// </summary>
    public bool ReturnsTable { get; }
    
    /// <summary>
    /// Whether to use named parameters.
    /// </summary>
    public bool UseNamedParameters { get; set; }
    
    /// <summary>
    /// The parameters of the routine.
    /// </summary>
    public IEnumerable<DbParameterDescriptor> Parameters { get; }

    /// <summary>
    /// Builds a statement to call the routine.
    /// </summary>
    /// <param name="provider">
    /// The provider to use.
    /// </param>
    /// <returns>
    /// The statement to call the routine.
    /// </returns>
    public string BuildStatement(DbProvider provider) => provider.BuildStatement(this);
}