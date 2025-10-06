var builder = WebApplication.CreateSlimBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services.AddHealthChecks();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddGrpc();
builder.Services.AddMagicOnion();

var app = builder.Build();

// app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
// app.UseCors("AllowAll");

app.UseInfrastructure();

app.MapMagicOnionService();

app.MapHealthChecks("/healthz");
app.MapGet("/", () => "Hello World!");

app.Run();
