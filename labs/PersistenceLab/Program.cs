using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Common.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

int Option(string key, int fallback, int maximum)
{
    var index = Array.IndexOf(args, key);
    if (index < 0) return fallback;
    if (index + 1 >= args.Length || !int.TryParse(args[index + 1], out var value) || value < 1 || value > maximum)
        throw new ArgumentException($"{key} must be between 1 and {maximum}.");
    return value;
}

var rows = Option("--rows", 2000, 100000);
var iterations = Option("--iterations", 50, 1000);
await using var database = new PostgreSqlBuilder("postgres:16.7").Build();
await database.StartAsync();
await using var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseNpgsql(database.GetConnectionString()).Options);
await context.Database.MigrateAsync();
var organization = Organization.Create("Persistence experiment", "Disposable benchmark data");
context.Add(organization);
context.Projects!.AddRange(Enumerable.Range(1, rows).Select(i => Project.Create($"Project {i:D6}", organization.Id)));
await context.SaveChangesAsync(default);
context.ChangeTracker.Clear();
await using var connection = new NpgsqlConnection(database.GetConnectionString());
var repository = new ProjectRepository(connection);

async Task EfRead()
{
    var count = await context.Projects.CountAsync();
    var page = await context.Projects.AsNoTracking().OrderBy(p => p.Id).Take(25).ToListAsync();
    if (count != rows || page.Count != Math.Min(25, rows)) throw new InvalidOperationException("EF result contract changed.");
}
async Task DapperRead()
{
    var page = await repository.GetAllAsync(new PaginationParams { PageSize = 25 });
    if (page.TotalCount != rows || page.Data!.Count() != Math.Min(25, rows)) throw new InvalidOperationException("Dapper result contract changed.");
}
for (var i = 0; i < 5; i++) { await EfRead(); await DapperRead(); }
var ef = new List<double>();
var dapper = new List<double>();
async Task Measure(Func<Task> read, List<double> samples)
{
    var watch = Stopwatch.StartNew();
    await read();
    samples.Add(watch.Elapsed.TotalMilliseconds);
}
for (var i = 0; i < iterations; i++)
{
    if (i % 2 == 0) { await Measure(EfRead, ef); await Measure(DapperRead, dapper); }
    else { await Measure(DapperRead, dapper); await Measure(EfRead, ef); }
}
object Summary(List<double> samples) => new { meanMs = samples.Average(), p95Ms = samples.Order().ElementAt((int)Math.Ceiling(samples.Count * .95) - 1) };
Console.WriteLine(JsonSerializer.Serialize(new
{
    rows, iterations, runtime = RuntimeInformation.FrameworkDescription, architecture = RuntimeInformation.ProcessArchitecture.ToString(),
    query = "ordered 25-row page plus total count", ef = Summary(ef), dapper = Summary(dapper),
    note = "Local educational measurement. EF uses two queries; Dapper batches the page and count. No universal speed claim."
}, new JsonSerializerOptions { WriteIndented = true }));
