namespace Infrastructure.UoW;

public class UnitOfWork(NpgsqlConnection connection) : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private NpgsqlTransaction? _transaction;
    
    // Cache table names to avoid repeated reflection calls
    private static readonly ConcurrentDictionary<Type, string> TableNameCache = new();

    public async Task BeginTransactionAsync()
    {
        await connection.OpenAsync();
        _transaction = await connection.BeginTransactionAsync();
    }

    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        var tableName = TableNameCache.GetOrAdd(typeof(T), type =>
            type.GetCustomAttribute<TableAttribute>()?.Name ?? type.Name + "s");
        return new DapperRepository<T>(connection, _transaction, tableName);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction. Call BeginTransactionAsync before committing.");

        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction. Call BeginTransactionAsync before rolling back.");

        await _transaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null) await _transaction.DisposeAsync();
        await connection.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}