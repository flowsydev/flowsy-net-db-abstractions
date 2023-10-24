namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a mechanism to instantiate units of work.
/// </summary>
public interface IUnitOfWorkFactory
{
    /// <summary>
    /// Creates a unit of work connected to a specific data store.
    /// </summary>
    /// <param name="dataStoreKey">
    /// A value that uniquely identifies the data store within all the data stores supported by the application.
    /// </param>
    /// <typeparam name="T">The type of unit of work.</typeparam>
    /// <returns>A new unit of work.</returns>
    T Create<T>(string dataStoreKey = "Default") where T : IUnitOfWork;
}