namespace Infrastructure.Common.Helpers;

public sealed class ConsoleSeedLogger : ILogger
{
    private readonly string _categoryName;

    public ConsoleSeedLogger(string categoryName) => _categoryName = categoryName;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var timestamp = DateTimeOffset.UtcNow.ToString("O", CultureInfo.InvariantCulture);
        Console.WriteLine("{0} [{1}] {2}: {3}", timestamp, logLevel, _categoryName, formatter(state, exception));
        if (exception is not null)
        {
            Console.WriteLine(exception);
        }
    }
}
