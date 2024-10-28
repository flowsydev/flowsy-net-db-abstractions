# Flowsy Db Abstractions

This package provides basic constructs for working with databases.

## DbProvider
The `DbProvider` enumeration contains values that specify the type of database provider to use. The following values are available:

- PostgreSql
- MySql
- SqlServer
- Oracle
- Sqlite


## DbProviderExtensions
The `DbProviderExtensions` class provides extension methods for the `DbProvider` enumeration. It has the following methods:

- GetInvariantName(): Returns the invariant name of the database provider.
- GetDefaultPort(): Returns the default port number for the database provider.
- GetDefaultDatabaseName(): Returns the default database name for the database provider.
- FormatCasting: Formats a string that casts a value to a specific type in the database provider's SQL dialect.
- SupportsEnums: Returns a boolean value that indicates whether the database provider supports enums.
- SupportsRoutineType: Returns a boolean value that indicates whether the database provider supports the specified routine type.
- RoutineCanReturnTable: Returns a boolean value that indicates whether the database provider supports routines that return tables.
- SupportsNamedParameters: Returns a boolean value that indicates whether the database provider supports named parameters.
- GetParameterPrefixForStatement: Returns the parameter prefix used by the database provider when building a statement.
- BuildStatement: Builds a SQL statement using the specified parameters.


## DbHost
The `DbHost` class holds information about the host of a database. It has the following properties:

- Provider: A value from the `DbProvider` enumeration that specifies the type of database provider to use.
- Address: A string that contains the host name or IP address of the database server.
- Port: An integer that contains the port number of the database server.


## DbCredentials
The `DbCredentials` class holds information about the credentials used to connect to a database. It has the following properties:

- Username: A string that contains the username used to connect to the database.
- Password: A string that contains the password used to connect to the database.


## DbConnectionOptions
The `DbConnectionOptions` class is used to configure a connection to a database. It has the following properties and methods:

- Provider: A value from the `DbProvider` enumeration that specifies the type of database provider to use.
- ConnectionString: A string that contains the connection string to the database.
- DatabaseName: A string that contains the name of the database to connect to.
- GetConnection(): Returns a `IDbConnection` to be used to interact with the database.


## DbRoutineDescriptor
The `DbRoutineDescriptor` class holds information about a database routine (e.g., stored procedure, function). It has the following properties:

- SchemaName: A string that contains the name of the schema that contains the routine.
- RoutineName: A string that contains the name of the routine.
- FullName: A string that contains the fully qualified name of the routine (i.e., `SchemaName.RoutineName`).
- Type: A value from the `DbRoutineType` enumeration that specifies the type of the routine (e.g., stored procedure, function).
- Parameters: A list of `DbParameterDescriptor` objects that describe the parameters of the routine.


## DbParameterDescriptor
The `DbParameterDescriptor` class holds information about a database parameter. It has the following properties:

- Name: A string that contains the name of the parameter.
- RuntimeType: A `Type` object that represents the runtime type of the parameter.
- DatabaseType: A value from the `DbType` enumeration that specifies the database type of the parameter.
- CustomType: A string that contains the custom type of the parameter (e.g., `MY_ENUM`).
- Direction: A value from the `ParameterDirection` enumeration that specifies the direction of the parameter (e.g., input, output).
- Size: An integer that contains the size of the parameter.
- Value: An object that contains the value of the parameter.
- ValueExpression: A value from the `DbValueExpression` enumeration that specifies how the value of the parameter should be interpreted.