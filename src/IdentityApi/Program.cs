var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cpnucleo Identity API", Version = "v1" });
});

builder.Services.AddHealthChecks();

builder.Services.AddSingleton<TokenGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHsts();

app.MapHealthChecks("/healthz");

app.MapPost("/login", ([FromBody] LoginRequest request, [FromServices] TokenGenerator tokenGenerator) => new
{
    access_token = tokenGenerator.GenerateToken(request.Email)
});

app.Run();
