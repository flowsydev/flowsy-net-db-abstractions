using Flowsy.Db.Abstractions.Test.Fixtures;
using Npgsql;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Flowsy.Db.Abstractions.Test;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class TableDescriptorTest : IClassFixture<PostgresDatabaseFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly PostgresDatabaseFixture _databaseFixture;
    private const DbProviderFamily ProviderFamily = DbProviderFamily.PostgreSql;

    public TableDescriptorTest(ITestOutputHelper output, PostgresDatabaseFixture databaseFixture)
    {
        _output = output;
        _databaseFixture = databaseFixture;
    }

    [Fact]
    [Priority(1)]
    public void Should_Register_Provider()
    {
        // Arrange
        const string providerName = "Npgsql";

        // Act
        var exception = Record.Exception(() => DbProvider.Register(providerName, ProviderFamily, NpgsqlFactory.Instance));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    [Priority(2)]
    public void Should_Create_Table()
    {
        // Arrange
        var connectionOptions = new DbConnectionOptions(_databaseFixture.ConnectionString, ProviderFamily);

        // Act
        var exception = Record.Exception(() =>
        {
            using var connection = connectionOptions.GetConnection(open: true);
            using var command = connection.CreateCommand();
            command.CommandText =
                $"""
                 CREATE TYPE gender AS ENUM ('Male', 'Female');

                 CREATE TABLE IF NOT EXISTS patient (
                     patient_id SERIAL PRIMARY KEY NOT NULL,
                     first_name VARCHAR(100) NOT NULL,
                     middle_name VARCHAR(100) NOT NULL,
                     last_name VARCHAR(100) NOT NULL,
                     second_last_name VARCHAR(100) NOT NULL,
                     birthdate DATE NULL,
                     gender gender NULL
                 );
                 """;
            command.ExecuteNonQuery();
        });

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    [Priority(3)]
    public void Should_Describe_Table()
    {
        // Arrange
        var connectionOptions = new DbConnectionOptions(_databaseFixture.ConnectionString, ProviderFamily);
        using var connection = connectionOptions.GetConnection(open: true);

        // Act
        var table = connection.GetTableDescriptor("patient");
        var columns = table?.Columns.ToArray() ?? [];
        var line = new string('-', 80);
        foreach (var column in columns)
        {
            _output.WriteLine(line);
            _output.WriteLine(column.ColumnQualifiedName);
            _output.WriteLine(line);
            
            _output.WriteLine($"Name: {column.ColumnName}");
            _output.WriteLine($"Type: {column.DataType}");
            _output.WriteLine($"Ordinal Position: {column.OrdinalPosition}");
            _output.WriteLine($"Nullable: {column.IsNullable}");
            _output.WriteLine($"Default: {column.DefaultValue}");
            _output.WriteLine($"Max Length: {column.CharacterMaximumLength}");
            _output.WriteLine($"Precision: {column.NumericPrecision}");
            _output.WriteLine($"Scale: {column.NumericScale}");
            _output.WriteLine($"Array: {column.IsArray}");
            _output.WriteLine($"Generated: {column.IsGenerated}");
            _output.WriteLine($"Collation: {column.CollationName}");
            _output.WriteLine($"Domain: {column.DomainQualifiedName}");
            _output.WriteLine($"User Defined Type: {column.IsUserDefinedType}");
            _output.WriteLine($"UDT Qualified Name: {column.UdtQualifiedName}");
            _output.WriteLine($"DbType: {column.GetDbType()}");
            _output.WriteLine($"Runtime Type: {column.GetRuntimeType()}");
            
            _output.WriteLine(string.Empty);
        }
        
        // Assert
        Assert.NotNull(table);
        Assert.Equal("patient", table.TableName);
        Assert.Equal(7, columns.Length);
        Assert.Equal("patient_id", columns[0].ColumnName);
    }
}