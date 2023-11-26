namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a set of configurations for running database migrations.
/// </summary>
public class DbMigrationConfiguration
{
    public DbMigrationConfiguration()
        : this(string.Empty, null, string.Empty, null)
    {
    }

    public DbMigrationConfiguration(string sourceDirectory, string? metadataSchema, string metadataTable, string? initializationStatement)
    {
        SourceDirectory = sourceDirectory;
        MetadataSchema = metadataSchema;
        MetadataTable = metadataTable;
        InitializationStatement = initializationStatement;
    }

    /// <summary>
    /// The directory containing migration scripts.
    /// </summary>
    public string SourceDirectory { get; set; }
    
    /// <summary>
    /// The database schema containing the table to store migration metadata. 
    /// </summary>
    public string? MetadataSchema { get; set; }
    
    /// <summary>
    /// The name of the table to store migration metadata.
    /// </summary>
    public string MetadataTable { get; set; }
    
    /// <summary>
    /// An optional statement to be executed after running migrations.
    /// </summary>
    public string? InitializationStatement { get; set; }
}