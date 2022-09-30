using Cpnucleo.API.Configuration;
using Cpnucleo.API.Filters;
using Cpnucleo.Application;
using Cpnucleo.Domain;
using Cpnucleo.Infrastructure.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomain();
builder.Services.AddInfrastructureData();
builder.Services.AddApplication(builder.Configuration);

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

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

WebApplication app = builder.Build();

app.UseRouting();

app.UseCors("AllowCpcnuleoClients");

app.UseSwaggerConfig();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseApplication();

app.MapControllers();
app.Run();