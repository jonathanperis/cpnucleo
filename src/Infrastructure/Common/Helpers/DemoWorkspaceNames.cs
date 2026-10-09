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

    /// <summary>
    /// A read-only summary of the demo data: active tasks per active board column and the generated
    /// text still left. The migrator prints it, so deploy logs show what the database holds.
    /// </summary>
    public static async Task<string> ReportAsync(NpgsqlConnection connection, CancellationToken cancellationToken = default)
    {
        const string sql = """
            WITH shared AS (SELECT "Password", "Salt" FROM "Users" GROUP BY "Password", "Salt" HAVING count(*) >= 50)
            SELECT 'Demo workspace names: report board ['
                || COALESCE((SELECT string_agg(w."Name" || '=' || (SELECT count(*) FROM "Assignments" a WHERE a."WorkflowId" = w."Id" AND a."Active"),
                                               ', ' ORDER BY w."Order", w."Id")
                             FROM "Workflows" w WHERE w."Active"), '')
                || '], generated names left: organizations=' || (SELECT count(*) FROM "Organizations" WHERE "Name" LIKE ANY (@names))
                || ', projects=' || (SELECT count(*) FROM "Projects" WHERE "Name" LIKE ANY (@names))
                || ', impediments=' || (SELECT count(*) FROM "Impediments" WHERE "Name" LIKE ANY (@names))
                || ', tasks=' || (SELECT count(*) FROM "Assignments" WHERE "Name" LIKE ANY (@names))
                || ', time entries=' || (SELECT count(*) FROM "Appointments" WHERE "Description" LIKE '%!')
                || ', generated logins=' || (SELECT count(*) FROM "Users" u
                                             WHERE u."Login" NOT LIKE '%@%' AND (u."Password", u."Salt") IN (SELECT "Password", "Salt" FROM shared))
            """;
        var opened = connection.State != System.Data.ConnectionState.Open;
        if (opened) await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await using var command = new NpgsqlCommand(sql, connection) { CommandTimeout = 300 };
            command.Parameters.AddWithValue("names", BogusVerbs.Select(verb => $"% {verb} %").ToArray());
            return (string)(await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false))!;
        }
        finally
        {
            if (opened) await connection.CloseAsync().ConfigureAwait(false);
        }
    }

    // The "-ing" verbs of Bogus Hacker names ("monitor transmitting back-end").
    private static readonly string[] BogusVerbs =
    [
        "backing up", "bypassing", "calculating", "compressing", "connecting", "copying", "generating", "hacking",
        "indexing", "navigating", "overriding", "parsing", "programming", "quantifying", "synthesizing", "transmitting"
    ];

    private static string LoadScript()
    {
        using var stream = typeof(DemoWorkspaceNames).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Embedded resource {ResourceName} is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
