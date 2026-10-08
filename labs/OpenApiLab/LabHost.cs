using System.Text.Json.Serialization.Metadata;
using FastEndpoints;
using FastEndpoints.OpenApi;
using Infrastructure.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;

namespace OpenApiLab;

internal sealed record GeneratedDocument(string Name, int StatusCode, string Body);

/// <summary>
/// A minimal in-process host with exactly WebApi's endpoints and FastEndpoints.OpenApi instead of
/// FastEndpoints.Swagger. It never starts WebApi's Program, opens no database connection, and listens on a
/// loopback port only long enough to fetch the documents.
/// </summary>
internal static class LabHost
{
    /// <summary>The out-of-the-box document with WebApi's equivalent settings.</summary>
    public const string Equivalent = "v1";

    /// <summary>The same settings plus ports of WebApi's two NSwag processors and NSwag-style schema names.</summary>
    public const string Transformed = "v1-transformed";

    public static async Task<IReadOnlyList<GeneratedDocument>> GenerateAsync(bool alignDefaultValues, params string[] documents)
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Development",
            ContentRootPath = AppContext.BaseDirectory
        });
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls("http://127.0.0.1:0");

        // Endpoint construction (FastEndpoints instantiates each endpoint once while mapping) resolves the
        // endpoints' constructor dependencies. The data source is created but never opened.
        builder.Configuration["DB_CONNECTION_STRING"] = "Host=127.0.0.1;Port=1;Database=never-opened";
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<WebApi.Common.Services.ListingChangeNotifier>();
        builder.Services.AddSingleton(_ => new WebApi.Common.Services.PasswordChangeThrottle(TimeProvider.System));
        Application.DependencyInjection.AddApplication(builder.Services);
        Infrastructure.DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);

        if (alignDefaultValues)
            builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.TypeInfoResolver =
                (o.SerializerOptions.TypeInfoResolver ?? new DefaultJsonTypeInfoResolver()).WithAddedModifier(DefaultValueAlignment.Modify));

        builder.Services.AddFastEndpoints(WebApi.DiscoveredTypes.All);
        foreach (var document in documents)
            builder.Services.OpenApiDocument(o => Configure(o, document, transformers: document == Transformed));

        var app = builder.Build();
        // Same as WebApi's UseFastEndpoints block, minus Warmup (it would build every endpoint's dependencies)
        // and the validation ResponseBuilder (runtime only).
        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";
            c.Endpoints.NameGenerator = context => context.EndpointType.Namespace!.Split('.')[^1];
            c.Errors.ContentType = "application/json";
            c.Errors.ProducesMetadataType = typeof(ApiErrorResponse);
        });
        app.MapOpenApi();

        await app.StartAsync();
        try
        {
            using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()), Timeout = TimeSpan.FromSeconds(30) };
            var results = new List<GeneratedDocument>();
            foreach (var document in documents)
            {
                using var response = await client.GetAsync($"/openapi/{document}.json");
                results.Add(new GeneratedDocument(document, (int)response.StatusCode, await response.Content.ReadAsStringAsync()));
            }
            return results;
        }
        finally
        {
            await app.StopAsync();
            await app.DisposeAsync();
        }
    }

    private static void Configure(DocumentOptions o, string name, bool transformers)
    {
        o.DocumentName = name;
        o.Title = "Cpnucleo Web API";
        o.Version = "v1";
        o.EnableJWTBearerAuth = true;
        o.ShortSchemaNames = true;
        o.ExcludeNonFastEndpoints = true;
        // Endpoints tag themselves (WithTags), as in WebApi.
        o.AutoTagPathSegmentIndex = 0;
        o.TagDescriptions = tags =>
        {
            tags["Account"] = "The signed-in user's own profile and password.";
            tags["Appointments"] = "Manage appointments and scheduling records.";
            tags["Assignments"] = "Manage work assignments and ownership.";
            tags["AssignmentImpediments"] = "Track impediments attached to assignments.";
            tags["AssignmentTypes"] = "Manage assignment classification data.";
            tags["Impediments"] = "Manage project and workflow blockers.";
            tags["Organizations"] = "Manage tenant organizations.";
            tags["Projects"] = "Manage projects and project metadata.";
            tags["Users"] = "Manage users exposed by the Web API.";
            tags["UserAssignments"] = "Manage user-to-assignment relationships.";
            tags["UserProjects"] = "Manage user-to-project relationships.";
            tags["Workflows"] = "Manage workflow definitions and transitions.";
        };
        o.ConfigureOpenApi = openApi =>
        {
            // Microsoft.AspNetCore.OpenApi defaults to 3.1; the committed NSwag document is 3.0.
            openApi.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            // DocumentOptions has no description/contact/license: NSwag's PostProcess becomes a document transformer.
            openApi.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Description = "Authenticated REST API for Cpnucleo project, workflow, assignment, organization, and user management.";
                document.Info.Contact = new OpenApiContact { Name = "Cpnucleo API Support", Url = new Uri("https://cpnucleo.jonathanperis.tech") };
                document.Info.License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://cpnucleo.jonathanperis.tech") };
                document.Info.TermsOfService = new Uri("https://cpnucleo.jonathanperis.tech");
                return Task.CompletedTask;
            });
            if (!transformers) return;
            openApi.CreateSchemaReferenceId = Transformers.SchemaId;
            openApi.AddOperationTransformer(new Transformers.PaginationQuery());
            openApi.AddOperationTransformer(new Transformers.ErrorResponses());
        };
    }
}
