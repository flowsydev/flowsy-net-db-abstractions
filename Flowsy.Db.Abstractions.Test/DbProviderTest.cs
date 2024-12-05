using System.Data;
using MySql.Data.MySqlClient;
using Npgsql;
using Xunit.Abstractions;

namespace Flowsy.Db.Abstractions.Test;

public class DbProviderTest
{
    private readonly ITestOutputHelper _output;

    public DbProviderTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void Should_BuildRoutineStatements()
    {
        // Arrange
        const string npgsqlProviderName = "Npgsql";
        const string mysqlProviderName = "MySql.Data";
        
        DbProvider.Register(npgsqlProviderName, DbProviderFamily.PostgreSql, NpgsqlFactory.Instance);
        DbProvider.Register(mysqlProviderName, DbProviderFamily.MySql, MySqlClientFactory.Instance);
        
        var providers = new List<DbProvider>
        {
            DbProvider.GetInstance(DbProviderFamily.PostgreSql),
            DbProvider.GetInstance(DbProviderFamily.MySql)
        };

        var connectionStrings = new Dictionary<DbProviderFamily, string>
        {
            [DbProviderFamily.PostgreSql] = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres",
            [DbProviderFamily.MySql] = "Server=localhost;Port=3306;Database=mysql;Uid=root;Pwd=root"
        };
        
        DbParameterDescriptor[] parameters =
        [
            new ("p_some_int", 1, DbType.Int32),
            new ("p_some_string", "some string", DbType.String),
            new ("p_some_enum", SomeEnum.FirstValue, DbValueExpression.CustomTypeCast, DbType.String, "some_enum"),
            new ("p_some_datetime", DateTime.Now, DbType.DateTime)
        ];
        var storedProcedure = new DbRoutineDescriptor(
            "some_schema.some_procedure",
            DbRoutineType.StoredProcedure,
            true,
            true,
            parameters
            );
        var tableFunction = new DbRoutineDescriptor(
            "some_schema",
            "some_function",
            DbRoutineType.StoredFunction,
            true,
            true,
            parameters
            );
        var scalarFunction = new DbRoutineDescriptor(
            "some_schema",
            "some_function",
            DbRoutineType.StoredFunction,
            false,
            true,
            parameters
            );
        DbRoutineDescriptor[] routines = [storedProcedure, tableFunction, scalarFunction];
        
        // Act
        List<string> statements = [];
        var doubleLineSeparator = new string('=', 100);
        var singleLineSeparator = new string('-', 100);
        foreach (var provider in providers)
        {
            var connectionOptions = new DbConnectionOptions(provider, connectionStrings[provider.Family]);
            using var connection = connectionOptions.GetConnection();
            
            _output.WriteLine(doubleLineSeparator);
            _output.WriteLine("Connection string [{0}: {1}]", provider, connection.ConnectionString);
            _output.WriteLine(doubleLineSeparator);
            
            foreach (var routine in routines)
            {
                if (!provider.SupportsRoutineType(routine.Type))
                    continue;
                
                if (routine.ReturnsTable && !provider.RoutineCanReturnTable(routine.Type))
                    continue;
                
                var statement = routine.BuildStatement(provider);
                
                _output.WriteLine($"Provider: {provider}, {routine.Type}({(routine.ReturnsTable ? "table" : "scalar")}): {routine.FullName}{Environment.NewLine}{statement}");
                
                _output.WriteLine(singleLineSeparator);
                
                statements.Add(statement);
            }
            _output.WriteLine(string.Empty);
        }

        // Assert
        Assert.NotEmpty(statements);
    }
}