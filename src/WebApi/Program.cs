var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHsts();

app.MapHealthChecks("/healthz");

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
