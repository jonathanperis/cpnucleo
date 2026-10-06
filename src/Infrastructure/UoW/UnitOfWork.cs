namespace Infrastructure.UoW;

public class UnitOfWork(NpgsqlConnection connection, ICurrentUser currentUser, IAccessGuard accessGuard) : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private NpgsqlTransaction? _transaction;

    // Cache table names to avoid repeated reflection calls
    private static readonly ConcurrentDictionary<Type, string> TableNameCache = new();

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already active. Commit or roll it back first.");
        if (connection.State != System.Data.ConnectionState.Open) await connection.OpenAsync(cancellationToken);
        _transaction = await connection.BeginTransactionAsync(cancellationToken);
    }

    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        var tableName = TableNameCache.GetOrAdd(typeof(T), type =>
            type.GetCustomAttribute<TableAttribute>()?.Name ?? type.Name + "s");
        // Repositories read the transaction when each command runs, so a repository obtained
        // before BeginTransactionAsync still enlists in the transaction started afterwards.
        return new DapperRepository<T>(connection, () => _transaction, tableName, currentUser, accessGuard);
    }

    /// <summary>
    /// Runs infrastructure-owned SQL inside the current transaction without access checks. Only for
    /// writes the system performs on the caller's behalf (e.g. the creator's project membership).
    /// </summary>
    internal Task<int> ExecuteTrustedAsync(string sql, object parameters, CancellationToken cancellationToken)
    {
        if (_transaction is null)
            throw new InvalidOperationException("Trusted writes must run inside a transaction.");
        return connection.ExecuteAsync(new CommandDefinition(sql, parameters, _transaction, cancellationToken: cancellationToken));
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
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;

        try
        {
            if (_transaction.Connection is not null) await _transaction.RollbackAsync(CancellationToken.None);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
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
