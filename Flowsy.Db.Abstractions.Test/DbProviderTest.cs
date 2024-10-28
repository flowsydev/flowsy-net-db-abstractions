using System.Data;
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
        var providers = Enum.GetValues<DbProvider>();
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
        var separator = new string('=', 80);
        foreach (var provider in providers)
        {
            foreach (var routine in routines)
            {
                if (!provider.SupportsRoutineType(routine.Type))
                    continue;
                
                if (routine.ReturnsTable && !provider.RoutineCanReturnTable(routine.Type))
                    continue;
                
                var statement = routine.BuildStatement(provider);
                
                _output.WriteLine($"Provider: {provider}, {routine.Type}({(routine.ReturnsTable ? "table" : "scalar")}): {routine.FullName}{Environment.NewLine}{statement}");
                
                _output.WriteLine(separator);
                
                statements.Add(statement);
            }
        }

        // Assert
        Assert.NotEmpty(statements);
    }
}