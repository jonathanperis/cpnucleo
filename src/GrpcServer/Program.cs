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

builder.Services.AddHealthChecks();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddGrpc();
builder.Services.AddMagicOnion();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
// app.UseCors("AllowAll");

app.UseInfrastructure();

app.UseHttpsRedirection();
app.UseHsts();

app.MapMagicOnionService();

app.MapHealthChecks("/healthz");
app.MapGet("/", () => "Hello World!");

app.Run();
