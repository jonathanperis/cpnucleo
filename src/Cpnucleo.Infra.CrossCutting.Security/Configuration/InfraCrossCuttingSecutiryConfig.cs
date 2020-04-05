using Cpnucleo.Infra.CrossCutting.Security.Filters;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cpnucleo.Infra.CrossCutting.Security.Configuration
{
    public static class InfraCrossCuttingSecutiryConfig
    {
        public static void AddInfraCrossCuttingSecutirySetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICryptographyManager, CryptographyManager>();
            services.AddScoped<IJwtManager, JwtManager>();

            services.AddScoped<AuthorizerActionFilter>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = configuration["Jwt:Issuer"],
                           ValidAudience = configuration["Jwt:Issuer"],
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                       };
                   });
        }
    }
}
