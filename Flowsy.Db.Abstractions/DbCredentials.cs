namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents the credentials used to authenticate with a database.
/// </summary>
public sealed class DbCredentials
{
    public DbCredentials(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    /// <summary>
    /// Gets the user name used to authenticate with the database.
    /// </summary>
    public string UserName { get; }
    
    /// <summary>
    /// Gets the password used to authenticate with the database.
    /// </summary>
    public string Password { get; }
}