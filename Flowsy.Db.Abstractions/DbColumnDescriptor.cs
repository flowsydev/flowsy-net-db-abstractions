using System.Data;
using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a descriptor for a database column.
/// </summary>
public class DbColumnDescriptor
{
    public DbColumnDescriptor(
        string columnName,
        string dataType,
        int ordinalPosition,
        string tableName,
        string? tableCatalog = null,
        string? tableSchema = null,
        int? characterMaximumLength = null,
        int? numericPrecision = null,
        int? numericScale = null,
        bool isNullable = false,
        string? defaultValue = null,
        string? collationName = null,
        bool isGenerated = false,
        string? domainSchema = null,
        string? domainName = null,
        string? udtSchema = null,
        string? udtName = null
        )
    {
        ColumnName = columnName;
        DataType = dataType;
        OrdinalPosition = ordinalPosition;
        TableCatalog = tableCatalog;
        TableSchema = tableSchema;
        TableName = tableName;
        CharacterMaximumLength = characterMaximumLength;
        NumericPrecision = numericPrecision;
        NumericScale = numericScale;
        IsNullable = isNullable;
        DefaultValue = defaultValue;
        CollationName = collationName;
        IsGenerated = isGenerated;
        DomainSchema = domainSchema;
        DomainName = domainName;
        UdtSchema = udtSchema;
        UdtName = udtName;
    }

    /// <summary>
    /// Gets the catalog of the table.
    /// </summary>
    public string? TableCatalog { get; }
    
    /// <summary>
    /// Gets the schema of the table.
    /// </summary>
    public string? TableSchema { get; }
    
    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    public string TableName { get; } 
    
    /// <summary>
    /// Gets the fully qualified name of the table.
    /// </summary>
    public string TableQualifiedName => DbProvider.FormatQualifiedName(TableCatalog, TableSchema, TableName);
  
    /// <summary>
    /// Gets the name of the column.
    /// </summary>
    public string ColumnName { get; }

    /// <summary>
    /// Gets the full name of the column.
    /// </summary>
    public string ColumnQualifiedName => DbProvider.FormatQualifiedName(this);
    
    /// <summary>
    /// Gets the data type of the column.
    /// </summary>
    public string DataType { get; }
    
    /// <summary>
    /// Gets the ordinal position of the column.
    /// </summary>
    public int OrdinalPosition { get; }
    
    /// <summary>
    /// Gets the maximum length of the character data type.
    /// </summary>
    public int? CharacterMaximumLength { get; }
    
    /// <summary>
    /// Gets the numeric precision of the column.
    /// </summary>
    public int? NumericPrecision { get; }
    
    /// <summary>
    /// Gets the numeric scale of the column.
    /// </summary>
    public int? NumericScale { get; }
    
    /// <summary>
    /// Gets a value indicating whether the column is nullable.
    /// </summary>
    public bool IsNullable { get; }
    
    /// <summary>
    /// Gets the default value of the column.
    /// </summary>
    public string? DefaultValue { get; }
    
    /// <summary>
    /// Gets the collation name of the column.
    /// </summary>
    public string? CollationName { get; }

    /// <summary>
    /// Gets a value indicating whether the column is an array.
    /// </summary>
    public bool IsArray => DbDataTypes.IsArray(DataType);
    
    /// <summary>
    /// Gets a value indicating whether the column is computed.
    /// </summary>
    public bool IsGenerated { get; }
    
    /// <summary>
    /// Gets the schema of the domain.
    /// </summary>
    public string? DomainSchema { get; }
    
    /// <summary>
    /// Gets the name of the domain.
    /// </summary>
    public string? DomainName { get; }
    
    /// <summary>
    /// Gets the full name of the domain.
    /// </summary>
    public string? DomainQualifiedName 
    {
        get
        {
            var values = new List<string>();
            
            if (!string.IsNullOrEmpty(DomainSchema))
                values.Add(DomainSchema);
            
            if (!string.IsNullOrEmpty(DomainName))
                values.Add(DomainName);
            
            return values.Count != 0 ? DbProvider.FormatQualifiedName(values.ToArray()) : null;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the column type is a user-defined type.
    /// </summary>
    public bool IsUserDefinedType => DbDataTypes.IsUserDefinedType(DataType);
    
    /// <summary>
    /// Gets the schema of the user-defined type.
    /// </summary>
    public string? UdtSchema { get; }
    
    /// <summary>
    /// Gets the name of the user-defined type.
    /// </summary>
    public string? UdtName { get; }

    /// <summary>
    /// Gets the full name of the user-defined type.
    /// </summary>
    public string? UdtQualifiedName
    {
        get
        {
            var values = new List<string>();

            if (!string.IsNullOrEmpty(UdtSchema))
                values.Add(UdtSchema);
            
            if (!string.IsNullOrEmpty(UdtName))
                values.Add(UdtName);
            
            return values.Count != 0 ? DbProvider.FormatQualifiedName(values.ToArray()) : null;
        }
    }

    /// <summary>
    /// Gets the DbType of the column.
    /// </summary>
    /// <returns>
    /// The DbType of the column.
    /// </returns>
    public DbType GetDbType()
    {
        var dataTypeNormalized = DataType.ToLower();

        if (DbDataTypes.SmallInteger.Contains(dataTypeNormalized))
            return DbType.Int16;
        
        if (DbDataTypes.StandardInteger.Contains(dataTypeNormalized))
            return DbType.Int32;
        
        if (DbDataTypes.LargeInteger.Contains(dataTypeNormalized))
            return DbType.Int64;
        
        if (DbDataTypes.SinglePrecisionFloat.Contains(dataTypeNormalized))
            return DbType.Single;
        
        if (DbDataTypes.DoublePrecisionFloat.Contains(dataTypeNormalized))
            return DbType.Double;
        
        if (DbDataTypes.Decimal.Contains(dataTypeNormalized))
            return DbType.Decimal;
        
        if (DbDataTypes.Character.Contains(dataTypeNormalized))
            return DbType.String;
        
        if (DbDataTypes.Date.Contains(dataTypeNormalized))
            return DbType.Date;
        
        if (DbDataTypes.DateTime.Contains(dataTypeNormalized))
            return DbType.DateTime2;
        
        if (DbDataTypes.DateTimeOffset.Contains(dataTypeNormalized))
            return DbType.DateTimeOffset;
        
        if (DbDataTypes.Time.Contains(dataTypeNormalized))
            return DbType.Time;
        
        if (DbDataTypes.Boolean.Contains(dataTypeNormalized))
            return DbType.Boolean;
        
        if (DbDataTypes.UniqueIdentifier.Contains(dataTypeNormalized))
            return DbType.Guid;
        
        if (DbDataTypes.Binary.Contains(dataTypeNormalized))
            return DbType.Binary;
        
        if (DbDataTypes.Json.Contains(dataTypeNormalized) || DbDataTypes.Xml.Contains(dataTypeNormalized) || DbDataTypes.Enumerated.Contains(dataTypeNormalized) || DbDataTypes.Set.Contains(dataTypeNormalized))
            return DbType.String;

        return DbType.String;
    }
    
    /// <summary>
    /// Gets the runtime type of the column.
    /// </summary>
    /// <returns>
    /// The runtime type of the column.
    /// </returns>
    public Type GetRuntimeType()
    {
        if (!IsArray)
            return DbDataTypes.GetRuntimeType(DataType);
        
        if (string.IsNullOrEmpty(UdtName))
            throw new ArgumentException(Strings.ArrayColumnMustHaveUserDefinedType);
            
        var elementType = DbDataTypes.GetRuntimeType(DbDataTypes.GetSqlType(UdtName));
        return Array.CreateInstance(elementType, 0).GetType();
    }

    /// <summary>
    /// Parses a value according to the SQL type of the column.
    /// </summary>
    /// <param name="value">
    /// The value to parse.
    /// </param>
    /// <param name="arraySeparator">
    /// The separator for array values.
    /// </param>
    /// <returns>
    /// The parsed value.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the value cannot be parsed.
    /// </exception>
    public object? ParseValue(string? value, char arraySeparator = ',')
    {
        if (string.IsNullOrEmpty(value))
            return IsNullable ? null : throw new ArgumentException(Strings.ValueCannotBeNullOrEmptyForNonNullableColumns);

        if (IsUserDefinedType)
            return value;
        
        if (!IsArray)
            return DbDataTypes.ParseValue(DataType, value);
        
        if (string.IsNullOrEmpty(UdtName))
            throw new ArgumentException(Strings.ArrayColumnMustHaveUserDefinedType);
            
        return DbDataTypes.ParseArray(DbDataTypes.GetSqlType(UdtName), value, arraySeparator);
    }
}