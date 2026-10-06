var builder = WebApplication.CreateSlimBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = JwtKeys.ValidationParameters(builder.Configuration);
        options.Events = new JwtBearerEvents
        {
            // Same session rules as WebApi: deactivated accounts, changed credentials and revoked
            // admin claims stop working within the validator's cache window.
            OnTokenValidated = async context =>
            {
                var failure = await context.HttpContext.RequestServices.GetRequiredService<TokenSessionValidator>()
                    .ValidateAsync(context.Principal!, context.HttpContext.RequestAborted);
                if (failure is not null) context.Fail(failure);
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim(CpnucleoClaimTypes.Subject)
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

    options.OnRejected = (context, cancellationToken) => ApiErrorEnvelopeExtensions.WriteRateLimitRejectionAsync(
        context.HttpContext,
        context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter) ? retryAfter : null,
        "GrpcServer.RateLimiting",
        cancellationToken);
});

builder.Services.AddHealthChecks();

// HTTP/2 for gRPC traffic, HTTP/1.1 for healthchecks + diagnostics.
builder.WebHost.ConfigureKestrel(o =>
{
    o.ListenAnyIP(5020, lo => lo.Protocols = HttpProtocols.Http2);
    o.ListenAnyIP(5021, lo => lo.Protocols = HttpProtocols.Http1);
});

builder.AddHandlerServer();
builder.Services.AddGrpc(options => options.Interceptors.Add<GrpcServer.Common.Security.ValidationInterceptor>());
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// Traefik routes this host directly (one proxy hop); WebApi sits behind Traefik and NGINX (two).
builder.Services.Configure<Microsoft.AspNetCore.Builder.ForwardedHeadersOptions>(options => options.ForwardLimit = 1);

var app = builder.Build();

app.UseRateLimiter();

app.Use(async (context, next) =>
{
    await next();

    if (context.Request.Path.Value?.Equals("/healthz", StringComparison.OrdinalIgnoreCase) == true)
    {
        app.Logger.LogInformation("GET /healthz {StatusCode}", context.Response.StatusCode);
    }
});

app.UseHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { Predicate = _ => false });
app.UseHealthChecks("/readyz");

app.UseAuthentication();
app.UseAuthorization();
app.UseInfrastructure();

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

app.Run();
