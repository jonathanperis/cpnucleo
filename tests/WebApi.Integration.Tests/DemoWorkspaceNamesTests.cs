using System.Text;
using System.Text.RegularExpressions;
using Bogus;
using Dapper;
using Domain.Common;
using Domain.Common.Security;
using Domain.Entities;
using Infrastructure.Common.Helpers;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

namespace WebApi.Integration.Tests;

/// <summary>Runs the shared demo-data rename against a small Bogus-shaped dataset mixed with people's rows.</summary>
public class DemoWorkspaceNamesTests(IsolatedDatabase database) : IClassFixture<IsolatedDatabase>
{
    private static readonly Regex HackerName = new(
        "^(alarm|application|array|bandwidth|bus|capacitor|card|circuit|driver|feed|firewall|hard drive|interface|matrix|microchip|monitor|panel|pixel|port|program|protocol|sensor|system|transmitter) ",
        RegexOptions.CultureInvariant);

    private static readonly string[] Lifecycle = ["Id", "CreatedAt", "UpdatedAt", "DeletedAt", "Active"];

    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Rename_GivesGeneratedRowsWorkspaceNamesAndLeavesEverythingElseAlone()
    {
        var connectionString = await database.CreateDatabaseAsync();
        var seeded = await SeedAsync(connectionString);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(Cancellation);
        var lifecycleBefore = await SnapshotAsync(connection, lifecycleOnly: true);

        await using (var transaction = await connection.BeginTransactionAsync(Cancellation))
        {
            await DemoWorkspaceNames.ApplyAsync(connection, transaction, NullLogger.Instance, Cancellation);
            await transaction.CommitAsync(Cancellation);
        }

        (await SnapshotAsync(connection, lifecycleOnly: true)).ShouldBe(lifecycleBefore, "Ids, relations and lifecycle columns are kept");

        // People's rows and the demo account are untouched.
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Organizations" WHERE "Id" = @id""", new { id = seeded.PeopleOrganization })).ShouldBe("Acme Studio");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Projects" WHERE "Id" = @id""", new { id = seeded.PeopleProject })).ShouldBe("Website relaunch");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Assignments" WHERE "Id" = @id""", new { id = seeded.PeopleTask })).ShouldBe("Write release notes");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Description" FROM "Appointments" WHERE "Id" = @id""", new { id = seeded.PeopleEntry })).ShouldBe("Pairing session with Ana");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Impediments" WHERE "Id" = @id""", new { id = seeded.PeopleImpediment })).ShouldBe("Waiting for the client");
        (await connection.QuerySingleAsync<(string Name, string Login)>("""SELECT "Name", "Login" FROM "Users" WHERE "Id" = @id""", new { id = seeded.PeopleUser }))
            .ShouldBe(("Ana Souza", "ana@example.com"));
        (await connection.ExecuteScalarAsync<string>("""SELECT "Login" FROM "Users" WHERE "Id" = @id""", new { id = seeded.DemoUser })).ShouldBe(AdminLogins.DefaultLogin);

        // Generated rows read like a project workspace.
        (await connection.QueryAsync<string>("""SELECT "Name" FROM "Workflows" ORDER BY "Order" """))
            .ShouldBe(["Backlog", "To Do", "In Progress", "In Review", "Testing", "Done"]);
        (await connection.QueryAsync<string>("""SELECT "Name" FROM "AssignmentTypes" ORDER BY "Name" """)).ShouldBe(["Bug", "Chore", "Feature"]);
        foreach (var (table, column) in new[] { ("Organizations", "Name"), ("Projects", "Name"), ("Impediments", "Name"), ("Assignments", "Name") })
        {
            var names = (await connection.QueryAsync<string>($"""SELECT "{column}" FROM "{table}" """)).ToList();
            names.ShouldNotContain(name => HackerName.IsMatch(name), $"{table} still has generated names");
            if (table != "Assignments") names.Distinct().Count().ShouldBe(names.Count, $"{table} names are unique");
        }
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM (SELECT 1 FROM "Assignments" GROUP BY "ProjectId", "Name" HAVING count(*) > 1) x""")).ShouldBe(0);
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "Appointments" WHERE "Description" LIKE '%!'""")).ShouldBe(0);
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "AssignmentImpediments" WHERE "Description" LIKE '%!'""")).ShouldBe(0);

        var logins = (await connection.QueryAsync<string>("""SELECT "Login" FROM "Users" """)).ToList();
        logins.ShouldNotContain(login => login.StartsWith("learner-", StringComparison.Ordinal));
        logins.Select(login => login.Trim().ToLowerInvariant()).Distinct().Count().ShouldBe(logins.Count, "the twin learners get distinct logins");
        logins.Count(login => login.StartsWith("jordan.lee", StringComparison.Ordinal)).ShouldBe(2);

        // Every produced value still satisfies the domain factories.
        foreach (var row in await connection.QueryAsync<(string Name, string Description)>("""SELECT "Name", "Description" FROM "Organizations" """))
            Should.NotThrow(() => Organization.Create(row.Name, row.Description));
        foreach (var name in await connection.QueryAsync<string>("""SELECT "Name" FROM "Projects" """))
            Should.NotThrow(() => Project.Create(name, Guid.NewGuid()));
        foreach (var row in await connection.QueryAsync<(string Name, string Description, DateTime StartDate, DateTime EndDate, int AmountHours)>(
                     """SELECT "Name", "Description", "StartDate", "EndDate", "AmountHours" FROM "Assignments" """))
            Should.NotThrow(() => Assignment.Create(row.Name, row.Description, row.StartDate, row.EndDate, row.AmountHours, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        foreach (var row in await connection.QueryAsync<(string Description, DateTime KeepDate, int AmountHours)>("""SELECT "Description", "KeepDate", "AmountHours" FROM "Appointments" """))
            Should.NotThrow(() => Appointment.Create(row.Description, row.KeepDate, row.AmountHours, Guid.NewGuid(), Guid.NewGuid()));
        foreach (var login in logins)
            Should.NotThrow(() => User.Create("Name", login, new PasswordHash("hash", "salt")));

        // Generated schedules: board columns follow the dates and time entries fall inside the worked window.
        (await connection.ExecuteScalarAsync<int>("""
            SELECT count(*) FROM "Assignments" a JOIN "Workflows" w ON w."Id" = a."WorkflowId"
            WHERE a."Id" <> @task AND (a."StartDate" > a."EndDate"
               OR (a."EndDate" < now()) <> (w."Name" = 'Done')
               OR (a."StartDate" > now()) <> (w."Name" IN ('Backlog', 'To Do')))
            """, new { task = seeded.PeopleTask })).ShouldBe(0);
        (await connection.ExecuteScalarAsync<int>("""
            SELECT count(*) FROM "Appointments" e JOIN "Assignments" a ON a."Id" = e."AssignmentId"
            WHERE e."Id" <> @entry AND (e."KeepDate" < a."StartDate" OR e."KeepDate" > a."EndDate" OR e."KeepDate" > now())
            """, new { entry = seeded.PeopleEntry })).ShouldBe(0);

        // A second run finds nothing generated and changes nothing.
        var everything = await SnapshotAsync(connection, lifecycleOnly: false);
        await DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation);
        (await SnapshotAsync(connection, lifecycleOnly: false)).ShouldBe(everything);
    }

    [Fact]
    public async Task Rename_RefusesToRepeatNamesWhenTheCatalogIsTooSmall()
    {
        var connectionString = await database.CreateDatabaseAsync();
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            await db.Database.MigrateAsync(Cancellation);
            db.AddRange(Enumerable.Range(0, 121).Select(i => Impediment.Create("sensor calculating haptic")));
            await db.SaveChangesAsync(Cancellation);
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(Cancellation);
        var error = await Should.ThrowAsync<PostgresException>(() => DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation));
        error.MessageText.ShouldContain("exceed the 120 available names");
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "Impediments" WHERE "Name" = 'sensor calculating haptic'""")).ShouldBe(121, "the failed run changes nothing");
    }

    [Fact]
    public async Task Migration_RenamesADatabaseSeededBeforeTheWorkspaceNames()
    {
        var connectionString = await database.CreateDatabaseAsync();
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            await Microsoft.EntityFrameworkCore.Infrastructure.AccessorExtensions.GetService<Microsoft.EntityFrameworkCore.Migrations.IMigrator>(db).MigrateAsync("20261007000200_RelationshipRestore", Cancellation);
            db.AddRange(Organization.Create("driver indexing cross-platform", "We need to navigate the cross-platform SAS firewall!"),
                Workflow.Create("transmitter copying virtual", 1), User.Create("Fanny Waters", "learner-007566", new PasswordHash("hash", "salt")));
            await db.SaveChangesAsync(Cancellation);
        }

        // The production upgrade path: the one-shot migrator applies pending migrations.
        await using (var db = IsolatedDatabase.Context(connectionString))
            await db.Database.MigrateAsync(Cancellation);

        await using var connection = new NpgsqlConnection(connectionString);
        (await connection.ExecuteScalarAsync<int>("""
            SELECT count(*) FROM "__EFMigrationsHistory"
            WHERE "MigrationId" IN ('20261008000100_DemoWorkspaceNames', '20261008000200_DemoWorkspaceConvergence')
            """)).ShouldBe(2);
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Organizations" """)).ShouldNotStartWith("driver ");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Workflows" """)).ShouldBe("Backlog");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Login" FROM "Users" """)).ShouldBe("fanny.waters@cpnucleo.example");
    }

    [Fact]
    public async Task Rename_ConvergesAProductionShapedDatabase()
    {
        // Production: hand-named board columns and types, Bogus user-name logins sharing the importer's
        // one password hash, and tasks an earlier version already renamed but never scheduled.
        var connectionString = await database.CreateDatabaseAsync();
        var faker = new Faker { Random = new Randomizer(42) };
        var shared = new PasswordHash("shared-hash", "shared-salt");
        var now = DateTime.UtcNow;
        Guid peopleUserId;
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            await db.Database.MigrateAsync(Cancellation);
            var organization = Organization.Create("alarm hacking bluetooth", faker.Hacker.Phrase());
            var columns = Board.Select((name, i) => Workflow.Create(name, i + 1)).ToList();
            var types = new[] { "Task", "Bug", "Chore" }.Select(name => AssignmentType.Create(name)).ToList();
            var fakes = Enumerable.Range(0, 60).Select(i => User.Create(faker.Name.FullName(), $"{faker.Internet.UserName()}{i}", shared)).ToList();
            var person = User.Create("Ana Souza", "ana@example.com", new PasswordHash("own-hash", "own-salt"));
            peopleUserId = person.Id;
            db.Add(organization);
            db.AddRange(columns);
            db.AddRange(types);
            db.AddRange(fakes);
            db.Add(person);
            var project = Project.Create("Checkout v2 Launch", organization.Id);
            db.Add(project);
            db.Add(UserProject.Create(fakes[0].Id, project.Id));
            for (var i = 0; i < 30; i++)
            {
                var start = now.AddDays(-faker.Random.Number(300, 700)).AddSeconds(faker.Random.Number(1, 3000));
                var description = i % 2 == 0
                    ? "Deliver CSV export in Checkout v2 Launch. Done when it is covered by tests, reviewed and demoed to the product owner."
                    : faker.Hacker.Phrase();
                var task = Assignment.Create(i % 2 == 0 ? $"Add CSV export {i}" : $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}",
                    description, start, start.AddDays(faker.Random.Number(100, 300)), faker.Random.Number(12, 60),
                    project.Id, columns[i % 3].Id, fakes[i % 60].Id, types[i % 3].Id);
                db.Add(task);
                db.Add(Appointment.Create("Wrote unit tests for CSV export.", start.AddDays(1), 2, task.Id, task.UserId));
            }
            await db.SaveChangesAsync(Cancellation);
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(Cancellation);
        await DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation);
        var checkedAt = DateTime.UtcNow;

        // Hand-named columns and types are kept; generated users get workspace logins, people keep theirs.
        (await connection.QueryAsync<string>("""SELECT "Name" FROM "Workflows" ORDER BY "Order" """)).ShouldBe(Board);
        (await connection.QueryAsync<string>("""SELECT "Name" FROM "AssignmentTypes" ORDER BY "Name" """)).ShouldBe(["Bug", "Chore", "Task"]);
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "Users" WHERE "Login" LIKE '%@%.example'""")).ShouldBe(60);
        (await connection.ExecuteScalarAsync<string>("""SELECT "Login" FROM "Users" WHERE "Id" = @id""", new { id = peopleUserId })).ShouldBe("ana@example.com");

        // Wording follows each type's name, and every task is scheduled into the column its dates imply.
        var tasks = (await connection.QueryAsync<(string Name, string Description, string Type, string Column, DateTime StartDate, DateTime EndDate)>("""
            SELECT a."Name", a."Description", t."Name", w."Name", a."StartDate", a."EndDate"
            FROM "Assignments" a JOIN "AssignmentTypes" t ON t."Id" = a."AssignmentTypeId" JOIN "Workflows" w ON w."Id" = a."WorkflowId"
            """)).ToList();
        tasks.Count.ShouldBe(30);
        tasks.Where(t => t.Type == "Bug").ShouldAllBe(t => t.Name.StartsWith("Fix ") && t.Description.StartsWith("Users of "));
        tasks.Where(t => t.Type == "Chore").ShouldAllBe(t => !t.Name.StartsWith("Fix ") && t.Description.StartsWith("Maintenance on "));
        tasks.Where(t => t.Type == "Task").ShouldAllBe(t => t.Description.StartsWith("Deliver "));
        tasks.ShouldAllBe(t => t.StartDate.TimeOfDay == TimeSpan.FromHours(9) && t.EndDate.TimeOfDay == TimeSpan.FromHours(17));
        ShouldFollowTheBoard(tasks.Select(t => (t.Column, t.StartDate, t.EndDate)), checkedAt);
        (await DemoWorkspaceNames.ReportAsync(connection, Cancellation)).ShouldEndWith(
            "generated names left: organizations=0, projects=0, impediments=0, tasks=0, time entries=0, generated logins=0");
        (await connection.ExecuteScalarAsync<int>("""
            SELECT count(*) FROM "Appointments" e JOIN "Assignments" a ON a."Id" = e."AssignmentId"
            WHERE e."KeepDate" < a."StartDate" OR e."KeepDate" > a."EndDate" OR e."KeepDate" > now()
            """)).ShouldBe(0);

        var everything = await SnapshotAsync(connection, lifecycleOnly: false);
        await DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation);
        (await SnapshotAsync(connection, lifecycleOnly: false)).ShouldBe(everything, "a repeated run changes nothing");
    }

    [Fact]
    public async Task Rename_RefusesToRepeatTaskNamesWithinAProjectAndType()
    {
        var connectionString = await database.CreateDatabaseAsync();
        var hash = new PasswordHash("hash", "salt");
        var now = DateTime.UtcNow;
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            await db.Database.MigrateAsync(Cancellation);
            var organization = Organization.Create("alarm hacking bluetooth", "We need to parse the optical SQL firewall!");
            var project = Project.Create("monitor transmitting back-end", organization.Id);
            var workflow = Workflow.Create("Doing", 1);
            var type = AssignmentType.Create("Feature");
            var user = User.Create("Ana Souza", "ana@example.com", hash);
            db.AddRange(organization, project, workflow, type, user);
            db.AddRange(Enumerable.Range(0, 481).Select(_ => Assignment.Create("firewall transmitting auxiliary", "We need to index the multi-byte CSS driver!",
                now.AddDays(-10), now.AddDays(-5), 12, project.Id, workflow.Id, user.Id, type.Id)));
            await db.SaveChangesAsync(Cancellation);
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(Cancellation);
        var error = await Should.ThrowAsync<PostgresException>(() => DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation));
        error.MessageText.ShouldContain("exceed the 480 names available per project and type");
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "Assignments" WHERE "Name" = 'firewall transmitting auxiliary'""")).ShouldBe(481);
    }

    [Fact]
    public async Task Rename_SwapsLoginsAndFallsBackForOrganizationsWithoutLatinLetters()
    {
        var connectionString = await database.CreateDatabaseAsync();
        var shared = new PasswordHash("shared-hash", "shared-salt");
        Guid jordan, mia, kenji;
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            await db.Database.MigrateAsync(Cancellation);
            // Jordan should get the login Mia holds, so the rename has to release it first.
            var jordanUser = User.Create("Jordan Lee", "Jordan42", shared);
            var miaUser = User.Create("Mia Chen", "jordan.lee@cpnucleo.example", shared);
            var kenjiUser = User.Create("Kenji Sato", "Kenji7", shared);
            var fillers = Enumerable.Range(0, 50).Select(i => User.Create($"Filler {(char)('a' + i % 26)}", $"Filler{i}", shared)).ToList();
            var organization = Organization.Create("日本商事", "Trading company");
            var project = Project.Create("Website relaunch", organization.Id);
            db.AddRange(jordanUser, miaUser, kenjiUser, organization, project);
            db.AddRange(fillers);
            db.Add(UserProject.Create(kenjiUser.Id, project.Id));
            await db.SaveChangesAsync(Cancellation);
            (jordan, mia, kenji) = (jordanUser.Id, miaUser.Id, kenjiUser.Id);
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(Cancellation);
        await DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation);

        async Task<string> Login(Guid id) => (await connection.ExecuteScalarAsync<string>("""SELECT "Login" FROM "Users" WHERE "Id" = @id""", new { id }))!;
        (await Login(jordan)).ShouldBe("jordan.lee@cpnucleo.example");
        (await Login(mia)).ShouldBe("mia.chen@cpnucleo.example");
        (await Login(kenji)).ShouldBe("kenji.sato@cpnucleo.example", "an organization slug without Latin letters falls back to cpnucleo");
    }

    [Fact]
    public async Task BoardMigration_RePlacesScheduledGeneratedTasksOnce()
    {
        // The state the convergence migration left in production: scheduled generated tasks whose
        // finished ones landed in the last active column, Blocked.
        var connectionString = await database.CreateDatabaseAsync();
        var now = DateTime.UtcNow;
        var today = now.Date;
        Guid manualTask;
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            await Microsoft.EntityFrameworkCore.Infrastructure.AccessorExtensions.GetService<Microsoft.EntityFrameworkCore.Migrations.IMigrator>(db)
                .MigrateAsync("20261008000200_DemoWorkspaceConvergence", Cancellation);
            var organization = Organization.Create("Lakeshore Bank", "Regional retail bank based in Amsterdam.");
            var project = Project.Create("Mobile Banking Redesign", organization.Id);
            var columns = Board.Select((name, i) => Workflow.Create(name, i + 1)).ToList();
            var type = AssignmentType.Create("Task");
            var user = User.Create("Ana Souza", "ana@example.com", new PasswordHash("own-hash", "own-salt"));
            db.AddRange(organization, project, type, user);
            db.AddRange(columns);
            const string description = "Deliver card freeze in Mobile Banking Redesign. Done when it is covered by tests, reviewed and demoed to the product owner.";
            db.AddRange(new[] { -200, -120, -40, -2, 20, 60 }
                .Select(offset => today.AddDays(offset).AddHours(9))
                .Select(start => Assignment.Create("Add card freeze", description, start, start.AddDays(5).AddHours(8), 24,
                    project.Id, columns[5].Id, user.Id, type.Id)));
            var manual = Assignment.Create("Add card freeze", description, today.AddDays(-300).AddHours(9), today.AddDays(-295).AddHours(17), 24,
                project.Id, columns[5].Id, user.Id, type.Id);
            manualTask = manual.Id;
            db.Add(manual);
            await db.SaveChangesAsync(Cancellation);
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(Cancellation);
        // A normal run keeps how scheduled tasks are placed (people may have moved them).
        await DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation);
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "Assignments" a JOIN "Workflows" w ON w."Id" = a."WorkflowId" WHERE w."Name" = 'Blocked'""")).ShouldBe(7);

        await using (var db = IsolatedDatabase.Context(connectionString))
            await db.Database.MigrateAsync(Cancellation);
        var checkedAt = DateTime.UtcNow;
        var tasks = (await connection.QueryAsync<(string Column, DateTime StartDate, DateTime EndDate)>("""
            SELECT w."Name", a."StartDate", a."EndDate" FROM "Assignments" a JOIN "Workflows" w ON w."Id" = a."WorkflowId"
            """)).ToList();
        ShouldFollowTheBoard(tasks, checkedAt);
        tasks.Select(t => t.Column).Distinct().Count().ShouldBeGreaterThan(2);

        // Once applied, later runs leave the board alone again.
        await connection.ExecuteAsync("""UPDATE "Assignments" SET "WorkflowId" = (SELECT "Id" FROM "Workflows" WHERE "Name" = 'Blocked') WHERE "Id" = @id""", new { id = manualTask });
        await DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation);
        (await connection.ExecuteScalarAsync<string>("""
            SELECT w."Name" FROM "Assignments" a JOIN "Workflows" w ON w."Id" = a."WorkflowId" WHERE a."Id" = @id
            """, new { id = manualTask })).ShouldBe("Blocked");
    }

    [Fact]
    public async Task Rename_RecognisesRecapitalisedBogusNamesAndReportsWhatIsLeft()
    {
        var connectionString = await database.CreateDatabaseAsync();
        Guid organizationId, generatedProject, suffixedProject, peopleProject;
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            await db.Database.MigrateAsync(Cancellation);
            var organization = Organization.Create("Driver Indexing Cross-Platform", "We need to navigate the cross-platform SAS firewall!");
            var generated = Project.Create("  Monitor  Transmitting Back-End ", organization.Id);
            var suffixed = Project.Create("feed indexing redundant x", organization.Id);
            var people = Project.Create("Learning programming basics", organization.Id);
            db.AddRange(organization, generated, suffixed, people);
            await db.SaveChangesAsync(Cancellation);
            (organizationId, generatedProject, suffixedProject, peopleProject) = (organization.Id, generated.Id, suffixed.Id, people.Id);
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(Cancellation);
        await DemoWorkspaceNames.ApplyAsync(connection, null, NullLogger.Instance, Cancellation);

        async Task<string> Name(string table, Guid id) => (await connection.ExecuteScalarAsync<string>($"""SELECT "Name" FROM "{table}" WHERE "Id" = @id""", new { id }))!;
        (await Name("Organizations", organizationId)).ShouldNotContain("Indexing", Case.Insensitive);
        (await Name("Projects", generatedProject)).ShouldNotContain("Transmitting", Case.Insensitive);
        (await Name("Projects", suffixedProject)).ShouldNotContain("indexing", Case.Insensitive);
        (await Name("Projects", peopleProject)).ShouldBe("Learning programming basics");
        (await DemoWorkspaceNames.ReportAsync(connection, Cancellation)).ShouldContain(
            "organizations=0, projects=1 [Learning programming basics], impediments=0, tasks=0");
    }

    // Production's board: the last active column is Blocked, which is not a stage.
    private static readonly string[] Board = ["Spec Ready", "Dev Ready", "In Progress", "Test Ready", "Done", "Blocked"];

    private static void ShouldFollowTheBoard(IEnumerable<(string Column, DateTime StartDate, DateTime EndDate)> tasks, DateTime checkedAt)
    {
        foreach (var (column, start, end) in tasks)
        {
            column.ShouldNotBe("Blocked");
            if (end < checkedAt) column.ShouldBe("Done");
            else if (start > checkedAt) column.ShouldBeOneOf("Spec Ready", "Dev Ready");
            else column.ShouldBeOneOf("In Progress", "Test Ready");
        }
    }

    private sealed record Seeded(Guid PeopleOrganization, Guid PeopleProject, Guid PeopleTask, Guid PeopleEntry, Guid PeopleImpediment, Guid PeopleUser, Guid DemoUser);

    private static async Task<Seeded> SeedAsync(string connectionString)
    {
        var faker = new Faker { Random = new Randomizer(20261008) };
        string HackerTriple() => $"{faker.Hacker.Noun()} {faker.Hacker.IngVerb()} {faker.Hacker.Adjective()}";
        var hash = new PasswordHash("hash", "salt");
        var now = DateTime.UtcNow;

        await using var db = IsolatedDatabase.Context(connectionString);
        await db.Database.MigrateAsync(Cancellation);

        var organizations = Enumerable.Range(0, 3).Select(_ => Organization.Create(HackerTriple(), faker.Hacker.Phrase())).ToList();
        var peopleOrganization = Organization.Create("Acme Studio", "A real customer");
        var workflows = Enumerable.Range(1, 6).Select(order => Workflow.Create(HackerTriple(), order)).ToList();
        var types = Enumerable.Range(0, 3).Select(_ => AssignmentType.Create(HackerTriple())).ToList();
        var impediments = Enumerable.Range(0, 5).Select(_ => Impediment.Create(HackerTriple())).ToList();
        var peopleImpediment = Impediment.Create("Waiting for the client");
        var learners = new[] { "Jordan Lee", "Jordan Lee", "Mia O'Connor-Smith", "Ravi Patel" }
            .Select((name, i) => User.Create(name, $"learner-{i:D6}", hash)).ToList();
        var peopleUser = User.Create("Ana Souza", "ana@example.com", hash);
        var demoUser = User.Create("Cpnucleo Demo", AdminLogins.DefaultLogin, hash);
        db.AddRange(organizations);
        db.AddRange(workflows);
        db.AddRange(types);
        db.AddRange(impediments);
        db.AddRange(learners);
        db.AddRange(peopleOrganization, peopleImpediment, peopleUser, demoUser);

        var projects = organizations.SelectMany(o => Enumerable.Range(0, 2).Select(_ => Project.Create(HackerTriple(), o.Id))).ToList();
        var peopleProject = Project.Create("Website relaunch", organizations[0].Id);
        db.AddRange(projects);
        db.Add(peopleProject);
        foreach (var (learner, i) in learners.Select((learner, i) => (learner, i)))
            db.Add(UserProject.Create(learner.Id, projects[i % 2].Id));

        Assignment? firstTask = null;
        foreach (var project in projects)
        {
            for (var i = 0; i < 40; i++)
            {
                var start = now.AddMonths(-faker.Random.Number(12, 30));
                var task = Assignment.Create(HackerTriple(), faker.Hacker.Phrase(), start, start.AddMonths(faker.Random.Number(8, 20)), faker.Random.Number(12, 60),
                    project.Id, faker.PickRandom(workflows).Id, faker.PickRandom(learners).Id, types[i % 3].Id);
                firstTask ??= task;
                db.Add(task);
                for (var e = 0; e < i % 3; e++)
                    db.Add(Appointment.Create(faker.Hacker.Phrase(), now.AddMonths(-faker.Random.Number(1, 10)), faker.Random.Number(1, 6), task.Id, task.UserId));
                if (i % 7 == 0)
                    db.Add(AssignmentImpediment.Create(faker.Hacker.Phrase(), task.Id, faker.PickRandom(impediments).Id));
            }
        }

        var peopleTask = Assignment.Create("Write release notes", "Summarize the sprint", now.AddDays(-3), now.AddDays(2), 4,
            peopleProject.Id, workflows[0].Id, peopleUser.Id, types[0].Id);
        var peopleEntry = Appointment.Create("Pairing session with Ana", now.AddDays(-1), 2, firstTask!.Id, peopleUser.Id);
        db.AddRange(peopleTask, peopleEntry);
        await db.SaveChangesAsync(Cancellation);

        return new Seeded(peopleOrganization.Id, peopleProject.Id, peopleTask.Id, peopleEntry.Id, peopleImpediment.Id, peopleUser.Id, demoUser.Id);
    }

    private static async Task<string> SnapshotAsync(NpgsqlConnection connection, bool lifecycleOnly)
    {
        var tables = new Dictionary<string, string[]>
        {
            ["Organizations"] = [], ["Projects"] = ["OrganizationId"], ["Workflows"] = ["Order"], ["AssignmentTypes"] = [], ["Impediments"] = [],
            ["Users"] = ["Name", "Password", "Salt"], ["UserProjects"] = ["UserId", "ProjectId"], ["UserAssignments"] = ["UserId", "AssignmentId"],
            ["Assignments"] = ["ProjectId", "UserId", "AssignmentTypeId", "AmountHours"],
            ["Appointments"] = ["AssignmentId", "UserId", "AmountHours"], ["AssignmentImpediments"] = ["AssignmentId", "ImpedimentId"],
        };
        var snapshot = new StringBuilder();
        foreach (var (table, kept) in tables)
        {
            var row = lifecycleOnly ? $"concat_ws('|', {string.Join(", ", Lifecycle.Concat(kept).Select(c => $"t.\"{c}\""))})" : "t::text";
            snapshot.AppendLine(await connection.ExecuteScalarAsync<string>($"""SELECT '{table}:' || count(*) || ':' || md5(coalesce(string_agg({row}, ',' ORDER BY t."Id"), '')) FROM "{table}" t"""));
        }
        return snapshot.ToString();
    }
}
