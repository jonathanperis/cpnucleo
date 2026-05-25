var builder = WebApplication.CreateSlimBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey configuration is missing."))),
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer configuration is missing."),
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience configuration is missing."),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 10,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                AutoReplenishment = true
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "text/plain";

        var retryAfterSeconds = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
            ? Math.Ceiling(retryAfter.TotalSeconds).ToString()
            : "60";
        context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds;

        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken);

        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("GrpcServer.RateLimiting");
        logger.LogWarning("Rate limit exceeded for IP: {IpAddress}",
            context.HttpContext.Connection.RemoteIpAddress);
    };
});

builder.Services.AddHealthChecks();

// HTTP/2 for gRPC traffic, HTTP/1.1 for healthchecks + diagnostics.
builder.WebHost.ConfigureKestrel(o =>
{
    o.ListenAnyIP(5020, lo => lo.Protocols = HttpProtocols.Http2);
    o.ListenAnyIP(5021, lo => lo.Protocols = HttpProtocols.Http1);
});

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

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseRateLimiter();

app.Use(async (context, next) =>
{
    await next();

    if (context.Request.Path.Value?.Equals("/healthz", StringComparison.OrdinalIgnoreCase) == true)
    {
        app.Logger.LogInformation("{Method} {Path} {StatusCode}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode);
    }
});

app.UseHealthChecks("/healthz");

app.UseInfrastructure();
app.UseAuthentication();
app.UseAuthorization();

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

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    // app.UseSwaggerGen();
}

app.Run();
