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
        var storedProcedure = new DbRoutineDescriptor(
            "some_schema",
            "some_procedure",
            DbRoutineType.StoredProcedure,
            true,
            true,
            [
                new DbParameterDescriptor("p_some_int", typeof(int), DbType.Int32, value: 1),
                new DbParameterDescriptor("p_some_string", typeof(string), DbType.String, value: "some string"),
                new DbParameterDescriptor("p_some_enum", typeof(SomeEnum), DbType.String, value: SomeEnum.FirstValue, customType: "some_enum", valueExpression: DbValueExpression.CustomTypeCast),
                new DbParameterDescriptor("p_some_datetime", typeof(DateTime), DbType.DateTime, value: DateTime.Now)
            ]
            );
        var tableFunction = new DbRoutineDescriptor(
            "some_schema",
            "some_function",
            DbRoutineType.StoredFunction,
            true,
            true,
            [
                new DbParameterDescriptor("p_some_int", typeof(int), DbType.Int32, value: 1),
                new DbParameterDescriptor("p_some_string", typeof(string), DbType.String, value: "some string"),
                new DbParameterDescriptor("p_some_enum", typeof(SomeEnum), DbType.String, value: SomeEnum.FirstValue, customType: "some_enum", valueExpression: DbValueExpression.CustomTypeCast),
                new DbParameterDescriptor("p_some_datetime", typeof(DateTime), DbType.DateTime, value: DateTime.Now)
            ]
            );
        var scalarFunction = new DbRoutineDescriptor(
            "some_schema",
            "some_function",
            DbRoutineType.StoredFunction,
            false,
            true,
            [
                new DbParameterDescriptor("p_some_int", typeof(int), DbType.Int32, value: 1),
                new DbParameterDescriptor("p_some_string", typeof(string), DbType.String, value: "some string"),
                new DbParameterDescriptor("p_some_enum", typeof(SomeEnum), DbType.String, value: SomeEnum.FirstValue, customType: "some_enum", valueExpression: DbValueExpression.CustomTypeCast),
                new DbParameterDescriptor("p_some_datetime", typeof(DateTime), DbType.DateTime, value: DateTime.Now)
            ]
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