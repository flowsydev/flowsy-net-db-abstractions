# Flowsy Db Abstractions

This package provides basic constructs for working with databases.


## DbProvider
The `DbProvider` class includes methods and properties that represent different database providers. It has the following properties:

- Register(): Registers a database provider factory and associates it with the specified `DbProviderFamily` and invariant name.
- GetInstance(): Returns an instance of the registered `DbProvider`.
- Family: A value from the `DbProviderFamily` enumeration that specifies the family of the database provider.
- InvariantName: A string that contains the invariant name of the database provider.
- DefaultPort: An integer that contains the default port number for the database provider.
- DefaultDatabaseName: A string that contains the default database name for the database provider.
- DefaulSchemaName: A string that contains the default schema name for the database provider.
- Factory: A `DbProviderFactory` object that can be used to create instances of `IDbConnection`, `IDbCommand`, `IDbDataParameter`, and other database objects.
- ParameterPrefixForStatement: A string that contains the parameter prefix used by the database provider when building a statement.
- SupportsSchemas: A boolean value that indicates whether the database provider supports schemas.
- SupportsNamedParameters: A boolean value that indicates whether the database provider supports named parameters.
- SupportsEnums: A boolean value that indicates whether the database provider supports enums.
- SupportsRoutineType(): A method that returns a boolean value that indicates whether the database provider supports the specified routine type.
- RoutineCanReturnTable(): A method that returns a boolean value that indicates whether the database provider supports routines that return tables.
- FormatQualifiedName(): A method that formats a string that represents a qualified name in the database provider's SQL dialect.
- FormatCasting(): A method that formats a string that casts a value to a specific type in the database provider's SQL dialect.
- FormatNamedParameter(): A method that formats a string that represents a named parameter in the database provider's SQL dialect.
- CreateConnectionStringBuilder(): A method that returns a `DbConnectionStringBuilder` object that can be used to build connection strings for the database provider.
- BuildConnectionString: A method that builds a connection string using the specified parameters.
- BuildStatement: A method that builds a SQL statement using the specified parameters.


## DbHost
The `DbHost` class holds information about the host of a database. It has the following properties and methods:

- Address: A string that contains the host name or IP address of the database server.
- Port: An integer that contains the port number of the database server.
- CreateConnectionStringBuilder(): Returns a `DbConnectionStringBuilder` object that can be used to build connection strings.
- BuildConnectionString(): Builds a connection string using the specified parameters.
- CreateConnectionOptions: Creates an instance of `DbConnectionOptions` using the specified parameters.


## DbCredentials
The `DbCredentials` class holds information about the credentials used to connect to a database. It has the following properties:

- UserName: A string that contains the username used to connect to the database.
- Password: A string that contains the password used to connect to the database.


## DbConnectionOptions
The `DbConnectionOptions` class is used to configure a connection to a database. It has the following properties and methods:

- Provider: An instance of the `DbProvider` class that specifies the type of database provider.
- Host: An instance of the `DbHost` class that specifies the host of the database.
- ConnectionString: A string that contains the connection string to the database.
- DatabaseName: A string that contains the name of the database to connect to.
- GetConnection(): Returns an instance of `IDbConnection` to be used to interact with the database.

## DbTableDescriptor
The `DbTableDescriptor` class holds information about a database table. It has the following properties:

- TableCatalog: A string that contains the name of the catalog that contains the table.
- TableSchema: A string that contains the name of the schema that contains the table.
- TableName: A string that contains the name of the table.
- TableQualifiedName: A string that contains the fully qualified name of the table (i.e., `table_catalog.table_schema.table_name`).
- Columns: A list of `DbColumnDescriptor` objects that describe the columns of the table.
- UniqueContraints: A dictionary that contains the unique constraints of the table.

## DbColumnDescriptor
The `DbColumnDescriptor` class holds information about a database table column. It has the following properties and methods:

- Provider: An instance of the `DbProvider` class that specifies the type of database provider.
- TableCatalog: A string that contains the name of the catalog that contains the table.
- TableSchema: A string that contains the name of the schema that contains the table.
- TableName: A string that contains the name of the table.
- TableQualifiedName: A string that contains the fully qualified name of the table (i.e., `table_catalog.table_schema.table_name`).
- ColumnName: A string that contains the name of the column.
- ColumnQualifiedName: A string that contains the fully qualified name of the column (i.e., `table_catalog.table_schema.table_name.column_name`).
- DataType: A value representing the data type declaration for the column.
- OrdinalPosition: An integer that contains the ordinal position of the column in the table.
- CharacterMaximumLength: An integer that contains the maximum length of the column.
- NumericPrecision: An integer that contains the precision of the column.
- NumericScale: An integer that contains the scale of the column.
- IsNullable: A boolean value that indicates whether the column allows NULL values.
- DefaultValue: A string that contains the default value of the column.
- CollationName: A string that contains the collation name of the column.
- IsArray: A boolean value that indicates whether the column is an array.
- IsGenerated: A boolean value that indicates whether the column is generated.
- DomainSchema: A string that contains the schema of the domain that the column is based on.
- DomainName: A string that contains the name of the domain that the column is based on.
- DomainQualifiedName: A string that contains the fully qualified name of the domain (i.e., `domain_schema.domain_name`).
- IsUserDefinedType: A boolean value that indicates whether the column is based on a user-defined type.
- UdtSchema: A string that contains the schema of the user-defined type that the column is based on.
- UdtName: A string that contains the name of the user-defined type that the column is based on.
- UdtQualifiedName: A string that contains the fully qualified name of the user-defined type (i.e., `udt_schema.udt_name`).
- GetDbType(): Returns a value from the `DbType` enumeration that represents the database type of the column.
- GetRuntimeType(): Returns a `Type` object that represents the runtime type of the column.
- ParseValue(): Parses the specified value and returns an object that represents the value of the column.

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