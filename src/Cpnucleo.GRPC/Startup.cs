using Cpnucleo.GRPC.Services;
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
using System;
using System.Security.Claims;
using System.Text;

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
            services.AddCpnucleoSetup();

            services.AddCodeFirstGrpc();

            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));

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
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<SistemaGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<ProjetoGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<ImpedimentoGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<TipoTarefaGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<TarefaGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<ApontamentoGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<WorkflowGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<RecursoGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<ImpedimentoTarefaGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<RecursoProjetoGrpcService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<RecursoTarefaGrpcService>().RequireCors("AllowAll");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("If you're looking for clients that implements this GRPC service, please look to https://cpnucleo-mvc.azurewebsites.net");
                });
            });
        }
    }
}
