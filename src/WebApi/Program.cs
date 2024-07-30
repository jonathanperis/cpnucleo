var builder = WebApplication.CreateSlimBuilder(args);

// builder.Services.ConfigureHttpJsonOptions(options =>
// {
//     options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
//     options.SerializerOptions.TypeInfoResolverChain.Insert(0, SourceGenerationContext.Default);
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAppointmentEndpoints();
app.MapAssignmentEndpoints();
app.MapAssignmentImpedimentEndpoints();
app.MapAssignmentTypeEndpoints();
app.MapImpedimentEndpoints();
app.MapOrganizationEndpoints();
app.MapProjectEndpoints();
app.MapUserEndpoints();
app.MapUserAssignmentEndpoints();
app.MapUserProjectEndpoints();
app.MapWorkflowEndpoints();

app.Run();
