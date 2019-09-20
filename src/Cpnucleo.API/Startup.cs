using Cpnucleo.API.Configuration;
using Cpnucleo.API.Filters;
using Cpnucleo.API.Utils;
using Cpnucleo.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cpnucleo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCpnucleoSetup();
            services.AddSwaggerConfig();
            services.AddVersionConfig();

            services.AddScoped<AuthorizerActionFilter>();
            services.AddScoped<IJwtManager, JwtManager>();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //       .AddJwtBearer(options =>
            //       {
            //           options.TokenValidationParameters = new TokenValidationParameters
            //           {
            //               ValidateIssuer = true,
            //               ValidateAudience = true,
            //               ValidateLifetime = true,
            //               ValidateIssuerSigningKey = true,
            //               ValidIssuer = Configuration["Jwt:Issuer"],
            //               ValidAudience = Configuration["Jwt:Issuer"],
            //               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //           };
            //       });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateLifetime = true
                       };
                   });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwaggerUIConfig();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
