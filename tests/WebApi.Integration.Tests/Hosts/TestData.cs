using Dapper;
using Domain.Common.Security;
using Domain.Entities;
using Grpc.Core;

namespace WebApi.Integration.Tests.Hosts;

/// <summary>A consistent graph of prerequisite rows, inserted as trusted system data.</summary>
public sealed record Graph(
    Organization Organization,
    Project Project,
    User User,
    Workflow Workflow,
    AssignmentType Type,
    Impediment Impediment,
    Assignment Assignment);

public static class TestData
{
    public static async Task<Graph> CreateGraphAsync(this WebAppFixture app, params TestAccount[] members)
    {
        var organization = Organization.Create("Prerequisite organization", "Prerequisite");
        var project = Project.Create($"Prerequisite {Guid.NewGuid():N}", organization.Id);
        var user = User.Create("Prerequisite", $"prerequisite-{Guid.NewGuid():N}", new PasswordHash("unused-test-hash", ""));
        var workflow = Workflow.Create("Prerequisite", 1);
        var type = AssignmentType.Create("Prerequisite");
        var impediment = Impediment.Create("Prerequisite");
        var assignment = Assignment.Create("Prerequisite", "Prerequisite", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1,
            project.Id, workflow.Id, user.Id, type.Id);
        await using (var db = app.CreateDbContext())
        {
            db.AddRange(organization, project, user, workflow, type, impediment, assignment);
            db.AddRange(members.Select(member => UserProject.Create(member.Id, project.Id)));
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        return new Graph(organization, project, user, workflow, type, impediment, assignment);
    }

    public static async Task<(bool Active, bool HasDeletedAt)> RowStateAsync(this WebAppFixture app, string table, Guid id)
    {
        await using var connection = app.CreateConnection();
        return await connection.QuerySingleAsync<(bool, bool)>(
            $"""SELECT "Active", "DeletedAt" IS NOT NULL FROM "{table}" WHERE "Id" = @id""", new { id });
    }
}

/// <summary>Executes any remote command by reflection, so gRPC tests can be data-driven.</summary>
public static class Remote
{
    private static readonly System.Reflection.MethodInfo ExecuteMethod = typeof(RemoteConnectionCoreExtensions).GetMethods()
        .First(method => method.Name == "RemoteExecuteAsync" && method.IsGenericMethodDefinition &&
                         method.GetParameters() is [{ ParameterType: var command }, { ParameterType: var options }] &&
                         command.IsGenericType && command.GetGenericTypeDefinition() == typeof(FastEndpoints.ICommand<>) &&
                         options == typeof(CallOptions));

    public static object Command(string resource, string operation, object values)
    {
        var name = operation switch
        {
            "List" => $"List{resource}sCommand",
            "Get" => $"Get{resource}ByIdCommand",
            _ => $"{operation}{resource}Command"
        };
        var type = typeof(GrpcServer.Contracts.Commands.Project.CreateProjectCommand).Assembly
            .GetType($"GrpcServer.Contracts.Commands.{resource}.{name}")
            ?? throw new InvalidOperationException($"No {operation} command for {resource}.");
        var command = Activator.CreateInstance(type)!;
        foreach (var property in values.GetType().GetProperties())
        {
            var target = type.GetProperty(property.Name) ?? throw new InvalidOperationException($"{type.Name} has no {property.Name}.");
            target.SetValue(command, property.GetValue(values));
        }

        return command;
    }

    public static async Task<object> ExecuteAsync(object command, CallOptions options, WebAppFixture? app = null)
    {
        var resultType = command.GetType().GetInterfaces()
            .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(FastEndpoints.ICommand<>))
            .GetGenericArguments()[0];
        var task = (Task)ExecuteMethod.MakeGenericMethod(resultType).Invoke(null, [command, options])!;
        try
        {
            await task;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown && app is not null)
        {
            throw new InvalidOperationException($"{command.GetType().Name} failed on the server: {app.FailureDetails}", ex);
        }
        return task.GetType().GetProperty("Result")!.GetValue(task)!;
    }

    public static T Get<T>(this object result, string property) =>
        (T)result.GetType().GetProperty(property)!.GetValue(result)!;
}
