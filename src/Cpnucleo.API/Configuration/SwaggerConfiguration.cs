using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
                    Description = "Cpnucleo example ASP.NET Core Web API (deprecated)",
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

                config.SwaggerDoc("v2", new Info
                {
                    Version = "v2",
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

                Dictionary<string, IEnumerable<string>> security = new Dictionary<string, IEnumerable<string>>
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

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseSwaggerUIConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    c.RoutePrefix = "swagger";
                }
            });
        }
    }
}
