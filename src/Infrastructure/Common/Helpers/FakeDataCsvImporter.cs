namespace Infrastructure.Common.Helpers;

public static class FakeDataCsvImporter
{
    private const string SeedVersion = "fake-data-csv-v3-20260528-tenant-scoped";
    private const string DefaultDemoLogin = "demo@cpnucleo.local";
    private const string DefaultDemoPassword = "CpnucleoDemo2026!";
    private const string DefaultDemoName = "Cpnucleo Demo";
    private static readonly Guid DefaultDemoUserId = Guid.Parse("0198a4a8-6d1f-7a54-9b1c-c9c430f2d001");

    private const int OrganizationCount = 686;
    private const int ProjectCount = 1_258;
    private const int ImpedimentCount = 114;
    private const int AssignmentTypeCount = 3;
    private const int WorkflowCount = 6;
    private const int UserCount = 11_154;
    private const int UserProjectCount = 24_400;
    private const int AssignmentCount = 464_587;
    private const int UserAssignmentCount = 363_554;
    private const int AssignmentImpedimentCount = 11_369;
    private const int AppointmentCount = 489_571;

    public static async Task RunAsync(string connectionString, ILogger logger, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        await EnsureSeedHistoryTableAsync(connection, cancellationToken).ConfigureAwait(false);
        if (await SeedAlreadyAppliedAsync(connection, cancellationToken).ConfigureAwait(false))
        {
            logger.LogInformation("Fake CSV seed {SeedVersion} already applied; skipping import.", SeedVersion);
            return;
        }

        logger.LogWarning("Starting canonical FakeData CSV import {SeedVersion}; existing demo data will be replaced.", SeedVersion);
        var startedAt = DateTimeOffset.UtcNow;

        await ResetDatabaseAsync(connection, cancellationToken).ConfigureAwait(false);

        var random = new Random(20260528);
        Randomizer.Seed = new Random(20260528);

        var passwordHasher = new Argon2PasswordHasher();
        var fakeUserPasswordHash = passwordHasher.Hash("FakeUser@123");
        var defaultDemoPasswordHash = passwordHasher.Hash(DefaultDemoPassword);

        var organizationIds = CreateIds(OrganizationCount);
        var projectIds = CreateIds(ProjectCount);
        var projectOrganizationIds = new Guid[ProjectCount];
        var impedimentIds = CreateIds(ImpedimentCount);
        var assignmentTypeIds = CreateIds(AssignmentTypeCount);
        var workflowIds = CreateIds(WorkflowCount);
        var userIds = CreateIds(UserCount);
        var userOrganizationIds = CreateRoundRobinTenancyAssignments(UserCount, organizationIds);
        var userOrganizationIndexMap = BuildIndexMap(userOrganizationIds);
        var assignmentIds = CreateIds(AssignmentCount);
        var assignmentOrganizationIds = new Guid[AssignmentCount];

        await ImportOrganizationsAsync(connection, organizationIds, logger, cancellationToken).ConfigureAwait(false);
        await ImportProjectsAsync(connection, projectIds, organizationIds, projectOrganizationIds, random, logger, cancellationToken).ConfigureAwait(false);
        var projectOrganizationIndexMap = BuildIndexMap(projectOrganizationIds);
        await ImportImpedimentsAsync(connection, impedimentIds, logger, cancellationToken).ConfigureAwait(false);
        await ImportAssignmentTypesAsync(connection, assignmentTypeIds, logger, cancellationToken).ConfigureAwait(false);
        await ImportWorkflowsAsync(connection, workflowIds, logger, cancellationToken).ConfigureAwait(false);
        await ImportUsersAsync(connection, userIds, fakeUserPasswordHash, logger, cancellationToken).ConfigureAwait(false);
        await InsertDefaultDemoUserAsync(connection, defaultDemoPasswordHash, cancellationToken).ConfigureAwait(false);
        await ImportUserProjectsAsync(connection, userIds, projectIds, projectOrganizationIds, projectOrganizationIndexMap, userOrganizationIds, random, logger, cancellationToken).ConfigureAwait(false);
        await ImportAssignmentsAsync(connection, assignmentIds, projectIds, projectOrganizationIds, workflowIds, userIds, userOrganizationIds, userOrganizationIndexMap, assignmentTypeIds, assignmentOrganizationIds, random, logger, cancellationToken).ConfigureAwait(false);
        await ImportUserAssignmentsAsync(connection, userIds, userOrganizationIndexMap, assignmentIds, assignmentOrganizationIds, random, logger, cancellationToken).ConfigureAwait(false);
        await ImportAssignmentImpedimentsAsync(connection, assignmentIds, impedimentIds, random, logger, cancellationToken).ConfigureAwait(false);
        await ImportAppointmentsAsync(connection, assignmentIds, assignmentOrganizationIds, userIds, userOrganizationIndexMap, random, logger, cancellationToken).ConfigureAwait(false);
        await MarkSeedAppliedAsync(connection, startedAt, cancellationToken).ConfigureAwait(false);

        logger.LogWarning(
            "Finished canonical FakeData CSV import {SeedVersion}: Organizations={OrganizationCount}, Projects={ProjectCount}, Impediments={ImpedimentCount}, AssignmentTypes={AssignmentTypeCount}, Workflows={WorkflowCount}, Users={UserCount}+demo, UserProjects={UserProjectCount}, Assignments={AssignmentCount}, UserAssignments={UserAssignmentCount}, AssignmentImpediments={AssignmentImpedimentCount}, Appointments={AppointmentCount} in {ElapsedSeconds:n1}s.",
            SeedVersion,
            OrganizationCount,
            ProjectCount,
            ImpedimentCount,
            AssignmentTypeCount,
            WorkflowCount,
            UserCount,
            UserProjectCount,
            AssignmentCount,
            UserAssignmentCount,
            AssignmentImpedimentCount,
            AppointmentCount,
            (DateTimeOffset.UtcNow - startedAt).TotalSeconds);
    }

    private static async Task EnsureSeedHistoryTableAsync(NpgsqlConnection connection, CancellationToken cancellationToken)
    {
        const string sql = """
                           CREATE TABLE IF NOT EXISTS "__FakeDataCsvImports" (
                               "Version" text NOT NULL PRIMARY KEY,
                               "StartedAt" timestamp with time zone NOT NULL,
                               "CompletedAt" timestamp with time zone NOT NULL
                           );
                           """;
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task<bool> SeedAlreadyAppliedAsync(NpgsqlConnection connection, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("SELECT EXISTS (SELECT 1 FROM \"__FakeDataCsvImports\" WHERE \"Version\" = @version);", connection);
        command.Parameters.AddWithValue("version", SeedVersion);
        return (bool)(await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false) ?? false);
    }

    private static async Task ResetDatabaseAsync(NpgsqlConnection connection, CancellationToken cancellationToken)
    {
        const string sql = """
                           TRUNCATE TABLE
                               "Appointments",
                               "AssignmentImpediments",
                               "UserAssignments",
                               "Assignments",
                               "UserProjects",
                               "Projects",
                               "Organizations",
                               "Workflows",
                               "AssignmentTypes",
                               "Impediments",
                               "Users"
                           RESTART IDENTITY CASCADE;
                           """;
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task InsertDefaultDemoUserAsync(NpgsqlConnection connection, PasswordHash defaultDemoPasswordHash, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "UpdatedAt", "DeletedAt", "Active")
                           VALUES (@id, @name, @login, @password, @salt, @createdAt, NULL, NULL, TRUE)
                           ON CONFLICT ("Id") DO UPDATE
                           SET "Name" = EXCLUDED."Name",
                               "Login" = EXCLUDED."Login",
                               "Password" = EXCLUDED."Password",
                               "Salt" = EXCLUDED."Salt",
                               "UpdatedAt" = now(),
                               "DeletedAt" = NULL,
                               "Active" = TRUE;
                           """;
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("id", DefaultDemoUserId);
        command.Parameters.AddWithValue("name", DefaultDemoName);
        command.Parameters.AddWithValue("login", DefaultDemoLogin);
        command.Parameters.AddWithValue("password", defaultDemoPasswordHash.Hash);
        command.Parameters.AddWithValue("salt", defaultDemoPasswordHash.Salt);
        command.Parameters.AddWithValue("createdAt", DateTimeOffset.UtcNow);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task MarkSeedAppliedAsync(NpgsqlConnection connection, DateTimeOffset startedAt, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "INSERT INTO \"__FakeDataCsvImports\" (\"Version\", \"StartedAt\", \"CompletedAt\") VALUES (@version, @startedAt, @completedAt);",
            connection);
        command.Parameters.AddWithValue("version", SeedVersion);
        command.Parameters.AddWithValue("startedAt", startedAt);
        command.Parameters.AddWithValue("completedAt", DateTimeOffset.UtcNow);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static Guid[] CreateIds(int count)
    {
        var ids = new Guid[count];
        for (var i = 0; i < ids.Length; i++)
        {
            ids[i] = BaseEntity.GetNewId();
        }

        return ids;
    }

    private static Guid[] CreateRoundRobinTenancyAssignments(int count, Guid[] organizationIds)
    {
        var assignments = new Guid[count];
        for (var i = 0; i < assignments.Length; i++)
        {
            assignments[i] = organizationIds[i % organizationIds.Length];
        }

        return assignments;
    }

    private static Dictionary<Guid, int[]> BuildIndexMap(Guid[] organizationIds)
    {
        return organizationIds
            .Select((organizationId, index) => new { organizationId, index })
            .GroupBy(x => x.organizationId)
            .ToDictionary(group => group.Key, group => group.Select(x => x.index).ToArray());
    }

    private static async Task ImportOrganizationsAsync(NpgsqlConnection connection, Guid[] ids, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"Organizations\" (\"Id\", \"Name\", \"Description\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < ids.Length; i++)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(
                ids[i],
                $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}",
                faker.Hacker.Phrase(),
                PastCreatedAt(faker),
                PastUpdatedAt(faker),
                active ? null : PastDeletedAt(faker),
                active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} organizations via CSV COPY.", ids.Length);
    }

    private static async Task ImportProjectsAsync(NpgsqlConnection connection, Guid[] ids, Guid[] organizationIds, Guid[] projectOrganizationIds, Random random, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"Projects\" (\"Id\", \"Name\", \"OrganizationId\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < ids.Length; i++)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(
                ids[i],
                $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}",
                projectOrganizationIds[i] = i < organizationIds.Length ? organizationIds[i] : Pick(organizationIds, random),
                PastCreatedAt(faker),
                PastUpdatedAt(faker),
                active ? null : PastDeletedAt(faker),
                active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} projects via CSV COPY.", ids.Length);
    }

    private static async Task ImportImpedimentsAsync(NpgsqlConnection connection, Guid[] ids, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"Impediments\" (\"Id\", \"Name\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < ids.Length; i++)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(ids[i], $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}", PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} impediments via CSV COPY.", ids.Length);
    }

    private static async Task ImportAssignmentTypesAsync(NpgsqlConnection connection, Guid[] ids, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"AssignmentTypes\" (\"Id\", \"Name\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < ids.Length; i++)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(ids[i], $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}", PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} assignment types via CSV COPY.", ids.Length);
    }

    private static async Task ImportWorkflowsAsync(NpgsqlConnection connection, Guid[] ids, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"Workflows\" (\"Id\", \"Name\", \"Order\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < ids.Length; i++)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(ids[i], $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}", i + 1, PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} workflows via CSV COPY.", ids.Length);
    }

    private static async Task ImportUsersAsync(NpgsqlConnection connection, Guid[] ids, PasswordHash passwordHash, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"Users\" (\"Id\", \"Name\", \"Login\", \"Password\", \"Salt\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < ids.Length; i++)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(ids[i], faker.Name.FullName(), faker.Internet.UserName(), passwordHash.Hash, passwordHash.Salt, PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} users via CSV COPY.", ids.Length);
    }

    private static async Task ImportUserProjectsAsync(NpgsqlConnection connection, Guid[] userIds, Guid[] projectIds, Guid[] projectOrganizationIds, IReadOnlyDictionary<Guid, int[]> projectOrganizationIndexMap, Guid[] userOrganizationIds, Random random, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"UserProjects\" (\"Id\", \"UserId\", \"ProjectId\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        var written = 0;
        foreach (var projectId in projectIds)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(BaseEntity.GetNewId(), DefaultDemoUserId, projectId, PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
            written++;
        }

        for (; written < UserProjectCount; written++)
        {
            var active = true;
            var userIndex = random.Next(userIds.Length);
            var userOrganizationId = userOrganizationIds[userIndex];
            var projectId = PickProjectForOrganization(projectIds, projectOrganizationIndexMap, userOrganizationId, random);
            await writer.WriteLineAsync(Csv(BaseEntity.GetNewId(), userIds[userIndex], projectId, PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} tenant-scoped user projects via CSV COPY, including demo access to all {ProjectCount} projects.", UserProjectCount, projectIds.Length);
    }

    private static async Task ImportAssignmentsAsync(NpgsqlConnection connection, Guid[] ids, Guid[] projectIds, Guid[] projectOrganizationIds, Guid[] workflowIds, Guid[] userIds, Guid[] userOrganizationIds, IReadOnlyDictionary<Guid, int[]> userOrganizationIndexMap, Guid[] assignmentTypeIds, Guid[] assignmentOrganizationIds, Random random, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"Assignments\" (\"Id\", \"Name\", \"Description\", \"StartDate\", \"EndDate\", \"AmountHours\", \"ProjectId\", \"WorkflowId\", \"UserId\", \"AssignmentTypeId\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < ids.Length; i++)
        {
            var active = true;
            var projectIndex = random.Next(projectIds.Length);
            var projectOrganizationId = projectOrganizationIds[projectIndex];
            assignmentOrganizationIds[i] = projectOrganizationId;
            await writer.WriteLineAsync(Csv(
                ids[i],
                $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}",
                faker.Hacker.Phrase(),
                AssignmentStartDate(faker),
                AssignmentEndDate(faker),
                faker.Random.Number(12, 60),
                projectIds[projectIndex],
                Pick(workflowIds, random),
                PickUserForOrganization(userIds, userOrganizationIndexMap, projectOrganizationId, random),
                Pick(assignmentTypeIds, random),
                PastCreatedAt(faker),
                PastUpdatedAt(faker),
                active ? null : PastDeletedAt(faker),
                active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} assignments via CSV COPY.", ids.Length);
    }

    private static async Task ImportUserAssignmentsAsync(NpgsqlConnection connection, Guid[] userIds, IReadOnlyDictionary<Guid, int[]> userOrganizationIndexMap, Guid[] assignmentIds, Guid[] assignmentOrganizationIds, Random random, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"UserAssignments\" (\"Id\", \"UserId\", \"AssignmentId\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < UserAssignmentCount; i++)
        {
            var active = true;
            var assignmentIndex = random.Next(assignmentIds.Length);
            var assignmentOrganizationId = assignmentOrganizationIds[assignmentIndex];
            await writer.WriteLineAsync(Csv(BaseEntity.GetNewId(), PickUserForOrganization(userIds, userOrganizationIndexMap, assignmentOrganizationId, random), assignmentIds[assignmentIndex], PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} user assignments via CSV COPY.", UserAssignmentCount);
    }

    private static async Task ImportAssignmentImpedimentsAsync(NpgsqlConnection connection, Guid[] assignmentIds, Guid[] impedimentIds, Random random, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"AssignmentImpediments\" (\"Id\", \"Description\", \"AssignmentId\", \"ImpedimentId\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < AssignmentImpedimentCount; i++)
        {
            var active = true;
            await writer.WriteLineAsync(Csv(BaseEntity.GetNewId(), faker.Hacker.Phrase(), Pick(assignmentIds, random), Pick(impedimentIds, random), PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} assignment impediments via CSV COPY.", AssignmentImpedimentCount);
    }

    private static async Task ImportAppointmentsAsync(NpgsqlConnection connection, Guid[] assignmentIds, Guid[] assignmentOrganizationIds, Guid[] userIds, IReadOnlyDictionary<Guid, int[]> userOrganizationIndexMap, Random random, ILogger logger, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        await using var writer = await connection.BeginTextImportAsync(
            "COPY \"Appointments\" (\"Id\", \"Description\", \"KeepDate\", \"AmountHours\", \"AssignmentId\", \"UserId\", \"CreatedAt\", \"UpdatedAt\", \"DeletedAt\", \"Active\") FROM STDIN WITH (FORMAT CSV)",
            cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < AppointmentCount; i++)
        {
            var active = true;
            var assignmentIndex = random.Next(assignmentIds.Length);
            var assignmentOrganizationId = assignmentOrganizationIds[assignmentIndex];
            await writer.WriteLineAsync(Csv(BaseEntity.GetNewId(), faker.Hacker.Phrase(), KeepDate(faker), faker.Random.Number(1, 6), assignmentIds[assignmentIndex], PickUserForOrganization(userIds, userOrganizationIndexMap, assignmentOrganizationId, random), PastCreatedAt(faker), PastUpdatedAt(faker), active ? null : PastDeletedAt(faker), active)).ConfigureAwait(false);
        }

        logger.LogInformation("Imported {Count} appointments via CSV COPY.", AppointmentCount);
    }

    private static Guid Pick(Guid[] values, Random random) => values[random.Next(values.Length)];

    private static Guid PickProjectForOrganization(Guid[] projectIds, IReadOnlyDictionary<Guid, int[]> projectOrganizationIndexMap, Guid organizationId, Random random)
    {
        if (!projectOrganizationIndexMap.TryGetValue(organizationId, out var indexes) || indexes.Length == 0)
        {
            throw new InvalidOperationException($"No project exists for organization {organizationId}.");
        }

        return projectIds[indexes[random.Next(indexes.Length)]];
    }

    private static Guid PickUserForOrganization(Guid[] userIds, IReadOnlyDictionary<Guid, int[]> userOrganizationIndexMap, Guid organizationId, Random random)
    {
        if (!userOrganizationIndexMap.TryGetValue(organizationId, out var indexes) || indexes.Length == 0)
        {
            throw new InvalidOperationException($"No user exists for organization {organizationId}.");
        }

        return userIds[indexes[random.Next(indexes.Length)]];
    }

    private static DateTimeOffset PastCreatedAt(Faker faker) => ToUtc(faker.Date.Between(DateTime.UtcNow.AddMonths(faker.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(faker.Random.Number(-24, -12))));

    private static DateTimeOffset PastUpdatedAt(Faker faker) => ToUtc(faker.Date.Between(DateTime.UtcNow.AddMonths(faker.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(faker.Random.Number(-6, -2))));

    private static DateTimeOffset PastDeletedAt(Faker faker) => ToUtc(faker.Date.Between(DateTime.UtcNow.AddMonths(faker.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(faker.Random.Number(-7, -6))));

    private static DateTimeOffset AssignmentStartDate(Faker faker) => ToUtc(faker.Date.Between(DateTime.UtcNow.AddMonths(faker.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(faker.Random.Number(-24, -12))));

    private static DateTimeOffset AssignmentEndDate(Faker faker) => ToUtc(faker.Date.Between(DateTime.UtcNow.AddMonths(faker.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(faker.Random.Number(-6, -2))));

    private static DateTimeOffset KeepDate(Faker faker) => ToUtc(faker.Date.Between(DateTime.UtcNow.AddMonths(faker.Random.Number(-11, -9)), DateTime.UtcNow.AddMonths(faker.Random.Number(-8, -6))));

    private static DateTimeOffset ToUtc(DateTime value) => new(DateTime.SpecifyKind(value, DateTimeKind.Utc));

    private static string Csv(params object?[] values) => string.Join(',', values.Select(ToCsvField));

    private static string ToCsvField(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var text = value switch
        {
            DateTimeOffset dateTimeOffset => dateTimeOffset.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            bool boolean => boolean.ToString().ToLowerInvariant(),
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty,
            _ => value.ToString() ?? string.Empty
        };

        return string.Concat('"', text.Replace("\"", "\"\"", StringComparison.Ordinal), '"');
    }
}
