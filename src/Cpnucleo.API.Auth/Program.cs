using Cpnucleo.API.Auth.Filters;
using Cpnucleo.API.Configuration;
using Cpnucleo.Application;
using Cpnucleo.Domain;
using Cpnucleo.Infra.CrossCutting.Bus;
using Cpnucleo.Infra.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomain();
builder.Services.AddInfraData();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfraCrossCuttingBus(builder.Configuration);

builder.Services.AddSwaggerConfig();
builder.Services.AddVersionConfig();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowCpcnuleoClients",
                      x =>
                      {
                          x.WithOrigins(builder.Configuration["AppSettings:UrlCpnucleoBlazor"])
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.UseRouting();

app.UseSwaggerConfig();
app.UseApplication();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowCpcnuleoClients");

app.MapControllers();
app.Run();