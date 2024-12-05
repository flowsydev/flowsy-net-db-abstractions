using System.Diagnostics.CodeAnalysis;
using Flowsy.Db.Abstractions.Resources;

namespace Flowsy.Db.Abstractions;

/// <summary>
/// Describes a database table.
/// </summary>
public class DbTableDescriptor
{
    private readonly Dictionary<string, DbColumnDescriptor> _columnDictionary;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DbTableDescriptor"/> class.
    /// </summary>
    /// <param name="tableCatalog">
    /// The catalog of the table.
    /// </param>
    /// <param name="tableSchema">
    /// The schema of the table.
    /// </param>
    /// <param name="tableName">
    /// The name of the table.
    /// </param>
    /// <param name="columns">
    /// The list of columns in the table.
    /// </param>
    /// <param name="uniqueColumnConstraints">
    /// The unique column constraints.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when a unique column constraint references a column that does not exist in the table.
    /// </exception>
    public DbTableDescriptor(
        string tableCatalog,
        string? tableSchema,
        string tableName,
        IEnumerable<DbColumnDescriptor> columns,
        IReadOnlyDictionary<string, IEnumerable<string>>? uniqueColumnConstraints = null
        )
    {
        TableCatalog = tableCatalog;
        TableSchema = tableSchema;
        TableName = tableName;
        
        Columns = columns.ToArray();
        _columnDictionary = Columns.ToDictionary(c => c.ColumnName);

        if (uniqueColumnConstraints is null)
        {
            UniqueColumnConstraints = new Dictionary<string, IEnumerable<DbColumnDescriptor>>();
            return;
        }

        var uniqueColumnNames = uniqueColumnConstraints.Values.SelectMany(s => s).Distinct();
        if ( ! uniqueColumnNames.All(name => _columnDictionary.ContainsKey(name)) )
            throw new ArgumentException(Strings.NameXIsNotAValidColumnName, nameof(uniqueColumnConstraints));
        
        UniqueColumnConstraints = uniqueColumnConstraints.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select(columnName => _columnDictionary[columnName])
        );
    }
    
    /// <summary>
    /// The catalog of the table.
    /// </summary>
    public string TableCatalog { get; }
    
    /// <summary>
    /// The schema of the table.
    /// </summary>
    public string? TableSchema { get; }
    
    /// <summary>
    /// The name of the table.
    /// </summary>
    public string TableName { get; }

    /// <summary>
    /// The fully qualified name of the table.
    /// </summary>
    public string TableQualifiedName => DbProvider.FormatQualifiedName(TableCatalog, TableSchema, TableName);
    
    /// <summary>
    /// The list of columns in the table.
    /// </summary>
    public IEnumerable<DbColumnDescriptor> Columns { get; }
    public IReadOnlyDictionary<string, IEnumerable<DbColumnDescriptor>> UniqueColumnConstraints { get; }
    
    public bool TryGetColumn(string columnName, [MaybeNullWhen(false)] out DbColumnDescriptor column)
        => _columnDictionary.TryGetValue(columnName, out column);
}