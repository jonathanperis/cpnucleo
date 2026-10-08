namespace Infrastructure.Common.Helpers;

/// <summary>
/// Turns the Bogus-generated demo dataset into coherent project-workspace names and schedules.
/// The SQL lives in <c>DemoWorkspaceNames.sql</c> so the importer, the legacy init directory and
/// <c>scripts/apply-demo-workspace-names.sh</c> share one implementation. It only rewrites rows
/// that still carry generated text, so it is idempotent and leaves people's data alone.
/// </summary>
public static class DemoWorkspaceNames
{
    public const string ResourceName = "Infrastructure.Common.Helpers.DemoWorkspaceNames.sql";

    public static string Script { get; } = LoadScript();

    public static async Task ApplyAsync(NpgsqlConnection connection, NpgsqlTransaction? transaction, ILogger logger, CancellationToken cancellationToken = default)
    {
        void LogNotice(object? sender, NpgsqlNoticeEventArgs args) => logger.LogInformation("{Notice}", args.Notice.MessageText);

        connection.Notice += LogNotice;
        try
        {
            await using var command = new NpgsqlCommand(Script, connection, transaction) { CommandTimeout = 0 };
            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            connection.Notice -= LogNotice;
        }
    }

    private static string LoadScript()
    {
        using var stream = typeof(DemoWorkspaceNames).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Embedded resource {ResourceName} is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
