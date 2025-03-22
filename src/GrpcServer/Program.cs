var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    {
         Title = "Cpnucleo Identity API", 
         Version = "v1",
         Description = "A sample project that implements the best praticles when building modern .NET projects",
         Contact = new OpenApiContact() { Name = "Jonathan Peris", Email = "jonathan.peris@somewhere.com" },
         License = new OpenApiLicense() { Name = "MIT", Url = new Uri( "https://opensource.org/licenses/MIT" ) }
    });
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
