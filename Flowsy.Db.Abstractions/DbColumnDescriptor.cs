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
        var sqlTypeNormalized = DataType.ToLower();

        if (DbDataTypes.SmallInteger.Contains(sqlTypeNormalized))
            return DbType.Int16;
        
        if (DbDataTypes.StandardInteger.Contains(sqlTypeNormalized))
            return DbType.Int32;
        
        if (DbDataTypes.LargeInteger.Contains(sqlTypeNormalized))
            return DbType.Int64;
        
        if (DbDataTypes.SinglePrecisionFloat.Contains(sqlTypeNormalized))
            return DbType.Single;
        
        if (DbDataTypes.DoublePrecisionFloat.Contains(sqlTypeNormalized))
            return DbType.Double;
        
        if (DbDataTypes.Decimal.Contains(sqlTypeNormalized))
            return DbType.Decimal;
        
        if (DbDataTypes.Character.Contains(sqlTypeNormalized))
            return DbType.String;
        
        if (DbDataTypes.DateTime.Contains(sqlTypeNormalized))
            return DbType.DateTime;
        
        if (DbDataTypes.Boolean.Contains(sqlTypeNormalized))
            return DbType.Boolean;
        
        if (DbDataTypes.UniqueIdentifier.Contains(sqlTypeNormalized))
            return DbType.Guid;
        
        if (DbDataTypes.Binary.Contains(sqlTypeNormalized))
            return DbType.Binary;
        
        if (DbDataTypes.Json.Contains(sqlTypeNormalized) || DbDataTypes.Xml.Contains(sqlTypeNormalized) || DbDataTypes.Enumerated.Contains(sqlTypeNormalized) || DbDataTypes.Set.Contains(sqlTypeNormalized))
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
        var sqlTypeNormalized = DataType.ToLower();

        if (DbDataTypes.SmallInteger.Contains(sqlTypeNormalized))
            return typeof(short);
        
        if (DbDataTypes.StandardInteger.Contains(sqlTypeNormalized))
            return typeof(int);
        
        if (DbDataTypes.LargeInteger.Contains(sqlTypeNormalized))
            return typeof(long);
        
        if (DbDataTypes.SinglePrecisionFloat.Contains(sqlTypeNormalized))
            return typeof(float);
        
        if (DbDataTypes.DoublePrecisionFloat.Contains(sqlTypeNormalized))
            return typeof(double);
        
        if (DbDataTypes.Decimal.Contains(sqlTypeNormalized))
            return typeof(decimal);
        
        if (DbDataTypes.Character.Contains(sqlTypeNormalized))
            return typeof(string);
        
        if (DbDataTypes.DateTime.Contains(sqlTypeNormalized))
            return typeof(DateTime);
        
        if (DbDataTypes.Boolean.Contains(sqlTypeNormalized))
            return typeof(bool);
        
        if (DbDataTypes.UniqueIdentifier.Contains(sqlTypeNormalized))
            return typeof(Guid);
        
        if (DbDataTypes.Binary.Contains(sqlTypeNormalized))
            return typeof(byte[]);
        
        if (DbDataTypes.Json.Contains(sqlTypeNormalized) || DbDataTypes.Xml.Contains(sqlTypeNormalized) || DbDataTypes.Enumerated.Contains(sqlTypeNormalized) || DbDataTypes.Set.Contains(sqlTypeNormalized))
            return typeof(string);
        
        return typeof(string);
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
            
        return DbDataTypes.ParseArray(DbDataTypes.GetSqlType(DataType), value, arraySeparator);
    }
}