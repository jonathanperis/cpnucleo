namespace Infrastructure.UoW;

public class UnitOfWork(NpgsqlConnection connection) : IUnitOfWork
{
    private NpgsqlTransaction? _transaction;

    public async Task BeginTransactionAsync()
    {
        await connection.OpenAsync();
        _transaction = await connection.BeginTransactionAsync();
    }

    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        var tableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T).Name + "s";
        return new DapperRepository<T>(connection, _transaction, tableName);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction. Call BeginTransactionAsync before committing.");

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await EndTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction. Call BeginTransactionAsync before rolling back.");

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await EndTransactionAsync();
        }
    }

    private async ValueTask EndTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    
        await connection.DisposeAsync();
    }
}