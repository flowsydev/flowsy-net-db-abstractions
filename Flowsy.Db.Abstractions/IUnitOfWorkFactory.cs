namespace Flowsy.Db.Abstractions;

/// <summary>
/// Provides a mechanism to create units of work.
/// </summary>
public interface IUnitOfWorkFactory
{
    /// <summary>
    /// Creates a new unit of work.
    /// </summary>
    /// <typeparam name="T">The type of unit of work.</typeparam>
    /// <returns></returns>
    T Create<T>() where T : IUnitOfWork;
}