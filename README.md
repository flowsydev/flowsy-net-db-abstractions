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
