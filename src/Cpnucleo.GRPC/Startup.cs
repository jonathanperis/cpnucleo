using System;
using System.Security.Claims;
using System.Text;
using Cpnucleo.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ProtoBuf.Grpc.Server;

namespace Cpnucleo.GRPC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCodeFirstGrpc();

            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));

            services.AddCpnucleoSetup();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.PrimarySid);
                    policy.RequireClaim(ClaimTypes.Hash);
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<SistemaService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<ProjetoService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<ImpedimentoService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<TipoTarefaService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<TarefaService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<ApontamentoService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<WorkflowService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<RecursoService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<ImpedimentoTarefaService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<RecursoProjetoService>().RequireCors("AllowAll");
                //endpoints.MapGrpcService<RecursoTarefaService>().RequireCors("AllowAll");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("If you're looking for clients that implements this GRPC service, please look to https://cpnucleo-pages-grpc.azurewebsites.net");
                });
            });
        }
    }
}
