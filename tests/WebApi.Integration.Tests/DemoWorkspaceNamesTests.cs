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
        (await connection.ExecuteScalarAsync<int>("""SELECT count(*) FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261008000100_DemoWorkspaceNames'""")).ShouldBe(1);
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Organizations" """)).ShouldNotStartWith("driver ");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Name" FROM "Workflows" """)).ShouldBe("Backlog");
        (await connection.ExecuteScalarAsync<string>("""SELECT "Login" FROM "Users" """)).ShouldBe("fanny.waters@cpnucleo.example");
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
