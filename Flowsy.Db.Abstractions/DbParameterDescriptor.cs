using System.Data;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a database parameter descriptor.
/// </summary>
public sealed class DbParameterDescriptor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbParameterDescriptor"/> class.
    /// </summary>
    /// <param name="name">
    /// The parameter name.
    /// </param>
    /// <param name="value">
    /// The parameter value.
    /// </param>
    /// <param name="databaseType">
    /// The parameter database type.
    /// </param>
    /// <param name="direction">
    /// The parameter direction.
    /// </param>
    /// <param name="size">
    /// The parameter size.
    /// </param>
    public DbParameterDescriptor(
        string name,
        object? value = null,
        DbType? databaseType = null,
        ParameterDirection? direction = null,
        int? size = null
        ) : this (name, value, DbValueExpression.Raw, databaseType, null, direction, size)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbParameterDescriptor"/> class.
    /// </summary>
    /// <param name="name">
    /// The parameter name.
    /// </param>
    /// <param name="value">
    /// The parameter value.
    /// </param>
    /// <param name="valueExpression">
    /// The parameter value expression.
    /// </param>
    /// <param name="databaseType">
    /// The parameter database type.
    /// </param>
    /// <param name="customType">
    /// The parameter custom type.
    /// </param>
    /// <param name="direction">
    /// The parameter direction.
    /// </param>
    /// <param name="size">
    /// The parameter size.
    /// </param>
    public DbParameterDescriptor(
        string name,
        object? value = null,
        DbValueExpression valueExpression = DbValueExpression.Raw,
        DbType? databaseType = null,
        string? customType = null,
        ParameterDirection? direction = null,
        int? size = null
        )
    {
        Name = name;
        Value = value;
        ValueExpression = valueExpression;
        RuntimeType = value?.GetType() ?? typeof(object);
        DatabaseType = databaseType;
        CustomType = customType;
        Direction = direction;
        Size = size;
    }

    /// <summary>
    /// The parameter name.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The parameter value.
    /// </summary>
    public object? Value { get; }
    
    /// <summary>
    /// The parameter value expression.
    /// </summary>
    public DbValueExpression ValueExpression { get; }
    
    /// <summary>
    /// The parameter runtime type.
    /// </summary>
    public Type RuntimeType { get; }
    
    /// <summary>
    /// The parameter database type.
    /// </summary>
    public DbType? DatabaseType { get; }
    
    public string? CustomType { get; }
    
    /// <summary>
    /// The parameter direction.
    /// </summary>
    public ParameterDirection? Direction { get; }
    
    /// <summary>
    /// The parameter size.
    /// </summary>
    public int? Size { get; }
}