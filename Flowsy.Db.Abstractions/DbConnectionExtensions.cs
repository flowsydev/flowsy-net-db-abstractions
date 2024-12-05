using System.Data;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Extensions for <see cref="IDbConnection"/>.
/// </summary>
public static class DbConnectionExtensions
{
    /// <summary>
    /// Get a table descriptor for the specified table.
    /// </summary>
    /// <param name="connection">
    /// The database connection.
    /// </param>
    /// <param name="tableName">
    /// The name of the table.
    /// </param>
    /// <param name="tableSchema">
    /// The schema of the table.
    /// </param>
    /// <param name="tableCatalog">
    /// The catalog of the table.
    /// </param>
    /// <returns>
    /// The table descriptor or <see langword="null"/> if the table does not exist.
    /// </returns>
    public static DbTableDescriptor? GetTableDescriptor(
        this IDbConnection connection,
        string tableName,
        string? tableSchema = null,
        string? tableCatalog = null
        )
        => connection.GetTableDescriptor(null, tableName, tableSchema, tableCatalog);
    
    /// <summary>
    /// Get a table descriptor for the specified table.
    /// </summary>
    /// <param name="connection">
    /// The database connection.
    /// </param>
    /// <param name="transaction">
    /// The transaction to use.
    /// </param>
    /// <param name="tableName">
    /// The name of the table.
    /// </param>
    /// <param name="tableSchema">
    /// The schema of the table.
    /// </param>
    /// <param name="tableCatalog">
    /// The catalog of the table.
    /// </param>
    /// <returns>
    /// The table descriptor or <see langword="null"/> if the table does not exist.
    /// </returns>
    public static DbTableDescriptor? GetTableDescriptor(
        this IDbConnection connection,
        IDbTransaction? transaction,
        string tableName,
        string? tableSchema = null,
        string? tableCatalog = null
        )
    {
        var finalTableCatalog = tableCatalog ?? connection.Database;
        
        // Check if table exists
        {
            const string tableExistsQuery =
                $"""
                SELECT
                    COUNT(*)
                FROM information_schema.tables
                WHERE
                    table_catalog = @TableCatalog AND
                    (@TableSchema IS NULL OR table_schema = @TableSchema) AND
                    table_name = @TableName
                """;
            
            using var tableExistsCommand = connection.CreateCommand();
            tableExistsCommand.Transaction = transaction;
            tableExistsCommand.CommandText = tableExistsQuery;
            tableExistsCommand.CommandType = CommandType.Text;
            
            var tableCatalogParameter = tableExistsCommand.CreateParameter();
            tableCatalogParameter.ParameterName = "@TableCatalog";
            tableCatalogParameter.Value = finalTableCatalog;
            tableCatalogParameter.DbType = DbType.String;
            tableExistsCommand.Parameters.Add(tableCatalogParameter);
            
            var tableSchemaParameter = tableExistsCommand.CreateParameter();
            tableSchemaParameter.ParameterName = "@TableSchema";
            tableSchemaParameter.Value = tableSchema as object ?? DBNull.Value;
            tableSchemaParameter.DbType = DbType.String;
            tableExistsCommand.Parameters.Add(tableSchemaParameter);
            
            var tableNameParameter = tableExistsCommand.CreateParameter();
            tableNameParameter.ParameterName = "@TableName";
            tableNameParameter.Value = tableName;
            tableNameParameter.DbType = DbType.String;
            tableExistsCommand.Parameters.Add(tableNameParameter);
            
            var tableExists = (long) tableExistsCommand.ExecuteScalar();
            if (tableExists == 0)
                return null;
        }
    
        // Get columns
        var columns = new List<DbColumnDescriptor>();
        {
            const string columnInfoQuery =
                $"""
                 SELECT
                     c.column_name,
                     c.data_type,
                     c.ordinal_position,
                     c.table_name,
                     c.table_catalog,
                     c.table_schema,
                     c.character_maximum_length,
                     c.numeric_precision,
                     c.numeric_scale,
                     UPPER(c.is_nullable) != 'NO' AS is_nullable,
                     c.column_default AS default_value,
                     c.collation_name,
                     UPPER(c.is_generated) = 'ALWAYS' AS is_generated,
                     c.domain_schema,
                     c.domain_name,
                     c.udt_schema,
                     c.udt_name
                 FROM information_schema.columns AS c
                 WHERE
                    c.table_catalog = @TableCatalog AND
                    (@TableSchema IS NULL OR c.table_schema = @TableSchema) AND
                    c.table_name = @TableName
                 ORDER BY c.ordinal_position, c.column_name
                 """;
            
            using var columnInfoCommand = connection.CreateCommand();
            columnInfoCommand.Transaction = transaction;
            columnInfoCommand.CommandText = columnInfoQuery;
            columnInfoCommand.CommandType = CommandType.Text;
            
            var tableCatalogParameter = columnInfoCommand.CreateParameter();
            tableCatalogParameter.ParameterName = "@TableCatalog";
            tableCatalogParameter.Value = finalTableCatalog;
            tableCatalogParameter.DbType = DbType.String;
            columnInfoCommand.Parameters.Add(tableCatalogParameter);
            
            var tableSchemaParameter = columnInfoCommand.CreateParameter();
            tableSchemaParameter.ParameterName = "@TableSchema";
            tableSchemaParameter.Value = tableSchema as object ?? DBNull.Value;
            tableSchemaParameter.DbType = DbType.String;
            columnInfoCommand.Parameters.Add(tableSchemaParameter);
            
            var tableNameParameter = columnInfoCommand.CreateParameter();
            tableNameParameter.ParameterName = "@TableName";
            tableNameParameter.Value = tableName;
            tableNameParameter.DbType = DbType.String;
            columnInfoCommand.Parameters.Add(tableNameParameter);
            
            using var columnInfoReader = columnInfoCommand.ExecuteReader();
            while (columnInfoReader.Read())
            {
                var columnName = columnInfoReader.GetString(0);
                var dataType = columnInfoReader.GetString(1);
                var ordinalPosition = columnInfoReader.GetInt32(2);
                var columnTableName = columnInfoReader.GetString(3);
                var columnTableCatalog = columnInfoReader.GetString(4);
                var columnTableSchema = columnInfoReader.GetString(5);
                var characterMaximumLength = columnInfoReader.IsDBNull(6) ? null as int? : columnInfoReader.GetInt32(6);
                var numericPrecision = columnInfoReader.IsDBNull(7) ? null as int? : columnInfoReader.GetInt32(7);
                var numericScale = columnInfoReader.IsDBNull(8) ? null as int? : columnInfoReader.GetInt32(8);
                var isNullable = columnInfoReader.GetBoolean(9);
                var defaultValue = columnInfoReader.IsDBNull(10) ? null : columnInfoReader.GetString(10);
                var collationName = columnInfoReader.IsDBNull(11) ? null : columnInfoReader.GetString(11);
                var isGenerated = columnInfoReader.GetBoolean(12);
                var domainSchema = columnInfoReader.IsDBNull(13) ? null : columnInfoReader.GetString(13);
                var domainName = columnInfoReader.IsDBNull(14) ? null : columnInfoReader.GetString(14);
                var udtSchema = columnInfoReader.IsDBNull(15) ? null : columnInfoReader.GetString(15);
                var udtName = columnInfoReader.IsDBNull(16) ? null : columnInfoReader.GetString(16);

                var column = new DbColumnDescriptor(
                    columnName,
                    dataType,
                    ordinalPosition,
                    columnTableName,
                    columnTableCatalog,
                    columnTableSchema,
                    characterMaximumLength,
                    numericPrecision,
                    numericScale,
                    isNullable,
                    defaultValue,
                    collationName,
                    isGenerated,
                    domainSchema,
                    domainName,
                    udtSchema,
                    udtName
                    );
                columns.Add(column);
            }
        }
        
        // Get unique column constraints
        var uniqueColumnConstraints = new Dictionary<string, IEnumerable<string>>();
        {
            const string uniqueColumnInfoQuery = 
                $"""
                SELECT 
                    kcu.constraint_name, kcu.column_name
                FROM information_schema.table_constraints AS tc
                JOIN information_schema.key_column_usage AS kcu 
                    ON tc.table_name = kcu.table_name AND tc.constraint_name = kcu.constraint_name
                WHERE 
                    tc.table_catalog = @TableCatalog AND
                    (@TableSchema IS NULL OR tc.table_schema = @TableSchema) AND 
                    tc.table_name = @TableName AND
                    tc.constraint_type IN ('PRIMARY KEY', 'UNIQUE')
                ORDER BY 
                    tc.constraint_name, kcu.ordinal_position;
                """;
            
            using var uniqueColumnInfoCommand = connection.CreateCommand();
            uniqueColumnInfoCommand.Transaction = transaction;
            uniqueColumnInfoCommand.CommandText = uniqueColumnInfoQuery;
            uniqueColumnInfoCommand.CommandType = CommandType.Text;
            
            var tableCatalogParameter = uniqueColumnInfoCommand.CreateParameter();
            tableCatalogParameter.ParameterName = "@TableCatalog";
            tableCatalogParameter.Value = finalTableCatalog;
            tableCatalogParameter.DbType = DbType.String;
            uniqueColumnInfoCommand.Parameters.Add(tableCatalogParameter);
            
            var tableSchemaParameter = uniqueColumnInfoCommand.CreateParameter();
            tableSchemaParameter.ParameterName = "@TableSchema";
            tableSchemaParameter.Value = tableSchema as object ?? DBNull.Value;
            tableSchemaParameter.DbType = DbType.String;
            uniqueColumnInfoCommand.Parameters.Add(tableSchemaParameter);
            
            var tableNameParameter = uniqueColumnInfoCommand.CreateParameter();
            tableNameParameter.ParameterName = "@TableName";
            tableNameParameter.Value = tableName;
            tableNameParameter.DbType = DbType.String;
            uniqueColumnInfoCommand.Parameters.Add(tableNameParameter);
            
            using var columnInfoReader = uniqueColumnInfoCommand.ExecuteReader();
            while (columnInfoReader.Read())
            {
                var constraintName = columnInfoReader.GetString(0);
                var columnName = columnInfoReader.GetString(1);
                
                if (!uniqueColumnConstraints.TryGetValue(constraintName, out var columnsInConstraint))
                    columnsInConstraint = new List<string>();
                
                ((List<string>) columnsInConstraint).Add(columnName);
                
                uniqueColumnConstraints[constraintName] = columnsInConstraint;
            }
        }

        return new DbTableDescriptor(finalTableCatalog, tableSchema, tableName, columns, uniqueColumnConstraints);
    }
}