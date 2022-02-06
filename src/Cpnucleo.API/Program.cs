using Cpnucleo.API.Configuration;
using Cpnucleo.API.Filters;
using Cpnucleo.Infra.CrossCutting.IoC;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCpnucleoSetup(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddVersionConfig();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "AllowCpcnuleoClients",
//                      x =>
//                      {
//                          x.WithOrigins(builder.Configuration["AppSettings:UrlCpnucleoBlazor"])
//                            .AllowAnyHeader()
//                            .AllowAnyMethod();
//                      });
//});

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

// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

WebApplication app = builder.Build();

app.UseRouting();

app.UseSwaggerConfig();
app.UseCpnucleoApiSetup();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//app.UseCors("AllowCpcnuleoClients");

app.MapControllers();
app.Run();