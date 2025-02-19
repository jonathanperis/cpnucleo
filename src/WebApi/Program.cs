var builder = WebApplication.CreateSlimBuilder(args);

builder.ConfigureOpenTelemetry();
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    })
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        })
    .EnableApiVersionBinding();
    
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey("ForTheLoveOfGodStoreAndLoadThisSecurely"u8.ToArray()),
            ValidIssuer = "https://identity.peris-studio.dev",
            ValidAudience = "https://peris-studio.dev",
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });

builder.Services.AddHealthChecks();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        }
    );
    app.MapPrometheusScrapingEndpoint();
}

app.UseInfrastructure();

app.UseHttpsRedirection();
app.UseHsts();

app.MapHealthChecks("/healthz");

app.NewVersionedApi("Appointments")
    .MapAppointmentEndpoints();

app.NewVersionedApi("Assignments")
    .MapAssignmentEndpoints();

app.NewVersionedApi("AssignmentImpediments")
    .MapAssignmentImpedimentEndpoints();

app.NewVersionedApi("AssignmentTypes")
    .MapAssignmentTypeEndpoints();

app.NewVersionedApi("Impediments")
    .MapImpedimentEndpoints();

app.NewVersionedApi("Organizations")
    .MapOrganizationEndpoints();

app.NewVersionedApi("Projects")
    .MapProjectEndpoints();

app.NewVersionedApi("Users")
    .MapUserEndpoints();

app.NewVersionedApi("UserAssignments")
    .MapUserAssignmentEndpoints();

app.NewVersionedApi("UserProjects")
    .MapUserProjectEndpoints();

app.NewVersionedApi("Workflows")
    .MapWorkflowEndpoints();

app.Run();