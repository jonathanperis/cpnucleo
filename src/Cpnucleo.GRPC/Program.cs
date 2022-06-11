using Cpnucleo.Application;
using Cpnucleo.Domain;
using Cpnucleo.Infra.CrossCutting.Bus;
using Cpnucleo.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomain();
builder.Services.AddInfraData();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfraCrossCuttingBus(builder.Configuration);

builder.Services.AddGrpc();
builder.Services.AddMagicOnion();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.PrimarySid);
        policy.RequireClaim(ClaimTypes.Hash);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        };
    });

WebApplication app = builder.Build();

app.MigrateDatabase();
app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.UseCors(x =>
{
    // Apenas necessário para o SignalR. Configuração padrão do CORS se aplica para utilizar apenas com gRPC.
    x.WithOrigins(builder.Configuration["AppSettings:UrlCpnucleoMvc"])
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseApplication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapMagicOnionService().RequireCors("AllowAll");

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});

app.Run();