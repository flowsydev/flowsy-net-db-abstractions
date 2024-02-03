namespace Flowsy.Db.Abstractions;

/// <summary>
/// Represents a group of operations to be persisted only when all of them are done successfully.
/// Any changes made during the process shall be rolled back in case of error.
/// </summary>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Begins work to be persisted.
    /// </summary>
    void BeginWork();
    
    /// <summary>
    /// Event raised when the work for this unit has begun.
    /// </summary>
    event EventHandler? WorkBegun;
    
    /// <summary>
    /// Persists all the changes made in the context of the unit of work.
    /// </summary>
    void SaveWork();
    
    /// <summary>
    /// Asynchronously persists all the changes made in the context of the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    Task SaveWorkAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Event raised when all the changes were saved successfully.
    /// </summary>
    event EventHandler? WorkSaved;
    
    /// <summary>
    /// Rolls back all the changes made in the context of the unit of work.
    /// This method shall be invoked when disposed.
    /// </summary>
    void DiscardWork();
    
    /// <summary>
    /// Asynchronously rolls back all the changes made in the context of the unit of work.
    /// This method shall be invoked when disposed.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    Task DiscardWorkAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Event raised when the changes were rolled back.
    /// </summary>
    event EventHandler? WorkDiscarded;
}