using GrpcServer.Handlers.Appointment;
using GrpcServer.Handlers.Assignment;
using GrpcServer.Handlers.AssignmentImpediment;
using GrpcServer.Handlers.AssignmentType;
using GrpcServer.Handlers.Impediment;
using GrpcServer.Handlers.Organization;
using GrpcServer.Handlers.Project;
using GrpcServer.Handlers.User;
using GrpcServer.Handlers.UserAssignment;
using GrpcServer.Handlers.UserProject;
using GrpcServer.Handlers.Workflow;

var builder = WebApplication.CreateSlimBuilder(args);

var logger = LoggerFactory.Create(logging =>
{
    _ = logging.AddApplicationInsights();
}).CreateLogger<Program>();

builder.ConfigureOpenTelemetry();
builder.Logging.AddApplicationInsights();

// builder.Services.AddAuthorization();
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             IssuerSigningKey = new SymmetricSecurityKey("ForTheLoveOfGodStoreAndLoadThisSecurely"u8.ToArray()),
//             ValidIssuer = "https://identity.peris-studio.dev",
//             ValidAudience = "https://peris-studio.dev",
//             ValidateIssuerSigningKey = true,
//             ValidateLifetime = true,
//             ValidateIssuer = true,
//             ValidateAudience = true
//         };
//     });

// builder.Services.AddRateLimiter(options =>
// {
//     options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
//         RateLimitPartition.GetFixedWindowLimiter(
//             partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
//             factory: _ => new FixedWindowRateLimiterOptions
//             {
//                 PermitLimit = 50, // Allow 50 requests
//                 Window = TimeSpan.FromMinutes(1), // Per 1-minute window
//                 QueueLimit = 10, // Queue up to 10 additional requests
//                 QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Process oldest requests first
//                 AutoReplenishment = true // Default: automatically replenish permits
//             }));
//
//     options.OnRejected = async (context, cancellationToken) =>
//     {
//         // Custom rejection handling logic
//         context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
//         context.HttpContext.Response.Headers.RetryAfter = "60";
//
//         await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken);
//
//         // Optional logging
//         logger.LogWarning("Rate limit exceeded for IP: {IpAddress}",
//             context.HttpContext.Connection.RemoteIpAddress);
//     };
// });

builder.Services.AddHealthChecks();

// Accept only HTTP/2 to allow insecure connections for development.
builder.WebHost
    .ConfigureKestrel(o => o.ListenLocalhost(6000, o => o.Protocols = HttpProtocols.Http2));

builder.AddHandlerServer();

// builder.Services
//     // .AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes = WebApi.DiscoveredTypes.All)
//     .AddFastEndpoints()
//     .SwaggerDocument(o =>
//     {
//         o.DocumentSettings = s =>
//         {
//             s.Title = "Cpnucleo Web API";
//             s.Description = "A sample project that implements best practices when building modern .NET projects";
//             s.Version = "v1";
//         };
//         o.AutoTagPathSegmentIndex = 0; // Disable the auto-tagging by setting the AutoTagPathSegmentIndex property to 0
//     });

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure();

// app.
//     UseFastEndpoints()
//     .UseMiddleware<ElapsedTimeMiddleware>()
//     .UseMiddleware<ErrorHandlingMiddleware>();

app.MapHandlers(h =>
{
    h.Register<CreateAppointmentCommand, CreateAppointmentHandler, CreateAppointmentResult>();
    h.Register<GetAppointmentByIdCommand, GetAppointmentByIdHandler, GetAppointmentByIdResult>();
    h.Register<ListAppointmentsCommand, ListAppointmentsHandler, ListAppointmentsResult>();
    h.Register<RemoveAppointmentCommand, RemoveAppointmentHandler, RemoveAppointmentResult>();
    h.Register<UpdateAppointmentCommand, UpdateAppointmentHandler, UpdateAppointmentResult>();
    
    h.Register<CreateAssignmentCommand, CreateAssignmentHandler, CreateAssignmentResult>();
    h.Register<GetAssignmentByIdCommand, GetAssignmentByIdHandler, GetAssignmentByIdResult>();
    h.Register<ListAssignmentsCommand, ListAssignmentsHandler, ListAssignmentsResult>();
    h.Register<RemoveAssignmentCommand, RemoveAssignmentHandler, RemoveAssignmentResult>();
    h.Register<UpdateAssignmentCommand, UpdateAssignmentHandler, UpdateAssignmentResult>();
    
    h.Register<CreateAssignmentImpedimentCommand, CreateAssignmentImpedimentHandler, CreateAssignmentImpedimentResult>();
    h.Register<GetAssignmentImpedimentByIdCommand, GetAssignmentImpedimentByIdHandler, GetAssignmentImpedimentByIdResult>();
    h.Register<ListAssignmentImpedimentsCommand, ListAssignmentImpedimentsHandler, ListAssignmentImpedimentsResult>();
    h.Register<RemoveAssignmentImpedimentCommand, RemoveAssignmentImpedimentHandler, RemoveAssignmentImpedimentResult>();
    h.Register<UpdateAssignmentImpedimentCommand, UpdateAssignmentImpedimentHandler, UpdateAssignmentImpedimentResult>();
    
    h.Register<CreateAssignmentTypeCommand, CreateAssignmentTypeHandler, CreateAssignmentTypeResult>();
    h.Register<GetAssignmentTypeByIdCommand, GetAssignmentTypeByIdHandler, GetAssignmentTypeByIdResult>();
    h.Register<ListAssignmentTypesCommand, ListAssignmentTypesHandler, ListAssignmentTypesResult>();
    h.Register<RemoveAssignmentTypeCommand, RemoveAssignmentTypeHandler, RemoveAssignmentTypeResult>();
    h.Register<UpdateAssignmentTypeCommand, UpdateAssignmentTypeHandler, UpdateAssignmentTypeResult>();
    
    h.Register<CreateImpedimentCommand, CreateImpedimentHandler, CreateImpedimentResult>();
    h.Register<GetImpedimentByIdCommand, GetImpedimentByIdHandler, GetImpedimentByIdResult>();
    h.Register<ListImpedimentsCommand, ListImpedimentsHandler, ListImpedimentsResult>();
    h.Register<RemoveImpedimentCommand, RemoveImpedimentHandler, RemoveImpedimentResult>();
    h.Register<UpdateImpedimentCommand, UpdateImpedimentHandler, UpdateImpedimentResult>();
    
    h.Register<CreateOrganizationCommand, CreateOrganizationHandler, CreateOrganizationResult>();
    h.Register<GetOrganizationByIdCommand, GetOrganizationByIdHandler, GetOrganizationByIdResult>();
    h.Register<ListOrganizationsCommand, ListOrganizationsHandler, ListOrganizationsResult>();
    h.Register<RemoveOrganizationCommand, RemoveOrganizationHandler, RemoveOrganizationResult>();
    h.Register<UpdateOrganizationCommand, UpdateOrganizationHandler, UpdateOrganizationResult>();
    
    h.Register<CreateProjectCommand, CreateProjectHandler, CreateProjectResult>();
    h.Register<GetProjectByIdCommand, GetProjectByIdHandler, GetProjectByIdResult>();
    h.Register<ListProjectsCommand, ListProjectsHandler, ListProjectsResult>();
    h.Register<RemoveProjectCommand, RemoveProjectHandler, RemoveProjectResult>();
    h.Register<UpdateProjectCommand, UpdateProjectHandler, UpdateProjectResult>();
    
    h.Register<CreateUserCommand, CreateUserHandler, CreateUserResult>();
    h.Register<GetUserByIdCommand, GetUserByIdHandler, GetUserByIdResult>();
    h.Register<ListUsersCommand, ListUsersHandler, ListUsersResult>();
    h.Register<RemoveUserCommand, RemoveUserHandler, RemoveUserResult>();
    h.Register<UpdateUserCommand, UpdateUserHandler, UpdateUserResult>();
    
    h.Register<CreateUserAssignmentCommand, CreateUserAssignmentHandler, CreateUserAssignmentResult>();
    h.Register<GetUserAssignmentByIdCommand, GetUserAssignmentByIdHandler, GetUserAssignmentByIdResult>();
    h.Register<ListUserAssignmentsCommand, ListUserAssignmentsHandler, ListUserAssignmentsResult>();
    h.Register<RemoveUserAssignmentCommand, RemoveUserAssignmentHandler, RemoveUserAssignmentResult>();
    h.Register<UpdateUserAssignmentCommand, UpdateUserAssignmentHandler, UpdateUserAssignmentResult>();
    
    h.Register<CreateUserProjectCommand, CreateUserProjectHandler, CreateUserProjectResult>();
    h.Register<GetUserProjectByIdCommand, GetUserProjectByIdHandler, GetUserProjectByIdResult>();
    h.Register<ListUserProjectsCommand, ListUserProjectsHandler, ListUserProjectsResult>();
    h.Register<RemoveUserProjectCommand, RemoveUserProjectHandler, RemoveUserProjectResult>();
    h.Register<UpdateUserProjectCommand, UpdateUserProjectHandler, UpdateUserProjectResult>();
    
    h.Register<CreateWorkflowCommand, CreateWorkflowHandler, CreateWorkflowResult>();
    h.Register<GetWorkflowByIdCommand, GetWorkflowByIdHandler, GetWorkflowByIdResult>();
    h.Register<ListWorkflowsCommand, ListWorkflowsHandler, ListWorkflowsResult>();
    h.Register<RemoveWorkflowCommand, RemoveWorkflowHandler, RemoveWorkflowResult>();
    h.Register<UpdateWorkflowCommand, UpdateWorkflowHandler, UpdateWorkflowResult>();
});

if (app.Environment.IsDevelopment())
{
    // app.UseSwaggerGen();
}

// app.MapApiClientEndpoint("/cs-client", c =>
//     {
//         c.SwaggerDocumentName = "v1";
//         c.Language = GenerationLanguage.CSharp;
//         c.ClientNamespaceName = "MyCompanyName";
//         c.ClientClassName = "MyCsClient";
//     },
//     o =>
//     {
//         o.CacheOutput(p => p.Expire(TimeSpan.FromDays(365))); //cache the zip
//         o.ExcludeFromDescription();
//     });
//
// await app.GenerateApiClientsAndExitAsync(
//     c =>
//     {
//         c.SwaggerDocumentName = "v1"; //must match doc name above
//         c.Language = GenerationLanguage.CSharp;
//         c.OutputPath = Path.Combine(app.Environment.WebRootPath, "ApiClients", "CSharp");
//         c.ClientNamespaceName = "Cpnucleo.WebApi.Client";
//         c.ClientClassName = "Cpnucleo.WebApi.Client";
//         c.CreateZipArchive = true; //if you'd like a zip file as well
//     },
//     c =>
//     {
//         c.SwaggerDocumentName = "v1";
//         c.Language = GenerationLanguage.TypeScript;
//         c.OutputPath = Path.Combine(app.Environment.WebRootPath, "ApiClients", "Typescript");
//         c.ClientNamespaceName = "Cpnucleo.WebApi.Client";
//         c.ClientClassName = "cpnucleo-webapi-client";
//     });

app.Run();
