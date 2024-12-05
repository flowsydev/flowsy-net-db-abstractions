using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

public static class DbDataTypes
{
    public static readonly ISet<string> SmallInteger = new HashSet<string>
    {
        "smallint"
    };

    public static readonly ISet<string> StandardInteger = new HashSet<string>
    {
        "int", "integer", "mediumint"
    };

    public static readonly ISet<string> LargeInteger = new HashSet<string>
    {
        "bigint", "serial", "bigserial"
    };

    public static readonly ISet<string> Character = new HashSet<string>
    {
        "char", "character", "nchar", "varchar", "nvarchar", "varchar2", "character varying",
        "text", "mediumtext", "longtext", "clob", "nclob", "long"
    };
    
    public static readonly ISet<string> SinglePrecisionFloat = new HashSet<string>
    {
        "real"
    };

    public static readonly ISet<string> DoublePrecisionFloat = new HashSet<string>
    {
        "float", "double precision"
    };

    public static readonly ISet<string> Decimal = new HashSet<string>
    {
        "decimal", "numeric", "money", "smallmoney", "number"
    };

    public static readonly ISet<string> DateTime = new HashSet<string>
    {
        "date", "time", "datetime", "smalldatetime", "datetime2", "timestamp"
    };

    public static readonly ISet<string> Boolean = new HashSet<string>
    {
        "boolean", "bool", "bit"
    };
    
    public static readonly ISet<string> UniqueIdentifier = new HashSet<string>
    {
        "uuid", "uniqueidentifier"
    };

    public static readonly ISet<string> Binary = new HashSet<string>
    {
        "binary", "varbinary", "blob", "bytea", "mediumblob", "longblob", "raw", "image"
    };

    public static readonly ISet<string> Json = new HashSet<string>
    {
        "json", "jsonb"
    };
    
    public static readonly ISet<string> Xml = new HashSet<string>
    {
        "xml"
    };
    
    public static readonly ISet<string> Enumerated = new HashSet<string>
    {
        "enum"
    };
    
    public static readonly ISet<string> Set = new HashSet<string>
    {
        "set"
    };
    
    public static readonly ISet<string> UserDefined = new HashSet<string>
    {
        "user-defined"
    };
    
    /// <summary>
    /// Checks if the SQL type is a user-defined type.
    /// </summary>
    /// <param name="datalType">
    /// The SQL type to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the SQL type is a user-defined type; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsUserDefinedType(string datalType)
        => UserDefined.Contains(datalType.ToLowerInvariant());

    /// <summary>
    /// Checks if the SQL type is an array.
    /// </summary>
    /// <param name="datalType">
    /// The SQL type to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the SQL type is an array; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsArray(string datalType)
    {
        // Normalize SqlType to make the matching case-insensitive
        var datalTypeNormalized = datalType.ToLowerInvariant();

        // Check for common array indicators
        return datalTypeNormalized.Contains("[]") || datalTypeNormalized.StartsWith("array");
    }
    
    /// <summary>
    /// Parses a value according to a SQL type.
    /// </summary>
    /// <param name="dataType">
    /// The SQL type.
    /// </param>
    /// <param name="value">
    /// The value to parse.
    /// </param>
    /// <returns>
    /// The parsed value.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the value cannot be parsed.
    /// </exception>
    public static object? ParseValue(string dataType, string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        
        var datalTypeNormalized = dataType.ToLower();

        if (SmallInteger.Contains(datalTypeNormalized))
            return short.Parse(value);
            
        if (StandardInteger.Contains(datalTypeNormalized))
            return int.Parse(value);
            
        if (LargeInteger.Contains(datalTypeNormalized))
            return long.Parse(value);
            
        if (SinglePrecisionFloat.Contains(datalTypeNormalized))
            return float.Parse(value);
            
        if (DoublePrecisionFloat.Contains(datalTypeNormalized))
            return double.Parse(value);
            
        if (Decimal.Contains(datalTypeNormalized))
            return decimal.Parse(value);
            
        if (Character.Contains(datalTypeNormalized))
            return value;

        if (DateTime.Contains(datalTypeNormalized))
            return System.DateTime.Parse(value);
            
        if (Boolean.Contains(datalTypeNormalized))
            return bool.Parse(value);
            
        if (UniqueIdentifier.Contains(datalTypeNormalized))
            return Guid.Parse(value);
            
        if (Binary.Contains(datalTypeNormalized))
            return ParseHexadecimalValue(value);
            
        if (Json.Contains(datalTypeNormalized) || Xml.Contains(datalTypeNormalized) || Enumerated.Contains(datalTypeNormalized) || Set.Contains(datalTypeNormalized))
            return value;
            
        throw new ArgumentException(string.Format(Strings.CannotParseValueForSqlTypeX, dataType), nameof(value));
    }

    /// <summary>
    /// Parses an array value according to a SQL type.
    /// </summary>
    /// <param name="datalType">
    /// The SQL type.
    /// </param>
    /// <param name="value">
    /// The value to parse.
    /// </param>
    /// <param name="separator">
    /// The separator character.
    /// </param>
    /// <returns>
    /// The parsed array value.
    /// </returns>
    public static object?[] ParseArray(string datalType, string value, char separator = ',')
    {
        // Split by comma for array representation, and recursively parse elements
        var elements = value.Split(separator);
        return elements.Select(e => ParseValue(datalType, e)).ToArray();
    }

    /// <summary>
    /// Parses a hexadecimal value.
    /// </summary>
    /// <param name="value">
    /// The hexadecimal value to parse.
    /// </param>
    /// <returns>
    /// The parsed hexadecimal value.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the value cannot be parsed.
    /// </exception>
    public static byte[] ParseHexadecimalValue(string value)
    {
        if (value.Length % 2 != 0)
            throw new ArgumentException(Strings.HexStringMustHaveAnEvenNumberOfCharacters);
        
        var bytes = new byte[value.Length / 2];
        for (var i = 0; i < value.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
        }
        return bytes;
    }

    /// <summary>
    /// Gets the SQL type from a user-defined type name.
    /// </summary>
    /// <param name="udtName">
    /// The user-defined type name.
    /// </param>
    /// <returns>
    /// The SQL type.
    /// </returns>
    public static string GetSqlType(string udtName)
        => udtName.StartsWith("_") ? udtName[1..] : udtName;
}