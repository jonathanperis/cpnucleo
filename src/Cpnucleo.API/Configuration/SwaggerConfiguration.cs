using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cpnucleo.API.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Cpnucleo API",
                    Description = "Cpnucleo example ASP.NET Core Web API",
                    Contact = new Contact
                    {
                        Name = "Jonathan Peris",
                        Email = "jperis.silva@gmail.com",
                        Url = "https://jonathanperis.github.io",
                    },
                    License = new License
                    {
                        Name = "Use under MIT",
                        Url = "https://en.wikipedia.org/wiki/MIT_License",
                    }
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                config.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Informe o JWT recebido no login. Exemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                config.AddSecurityRequirement(security);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseSwaggerUIConfig(this IApplicationBuilder application)
        {
            application.UseSwagger();

            application.UseSwagger(swag =>
            {
                swag.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            application.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Cpnucleo V1");
                c.RoutePrefix = "swagger";
            });
        }
    }
}
