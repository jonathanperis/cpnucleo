namespace Infrastructure.UoW;

public class UnitOfWork(NpgsqlConnection connection) : IUnitOfWork
{
    private NpgsqlTransaction _transaction = null!;
    private NpgsqlConnection _connection = connection;

    public async Task BeginTransactionAsync()
    {
        await _connection.OpenAsync();
        _transaction = await _connection.BeginTransactionAsync();
    }

    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        var tableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T).Name + "s";
        return new DapperRepository<T>(_connection, _transaction, tableName);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await DisposeAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeAsync();
        }
    }

    private async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
    }

    public void Dispose() => DisposeAsync().GetAwaiter().GetResult();
}