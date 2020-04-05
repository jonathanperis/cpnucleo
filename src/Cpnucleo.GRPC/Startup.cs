using Cpnucleo.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddCpnucleoApiSetup();
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<SistemaService>();
                endpoints.MapGrpcService<ProjetoService>();
                endpoints.MapGrpcService<ImpedimentoService>();
                endpoints.MapGrpcService<TipoTarefaService>();
                endpoints.MapGrpcService<TarefaService>();
                endpoints.MapGrpcService<ApontamentoService>();
                endpoints.MapGrpcService<WorkflowService>();
                endpoints.MapGrpcService<RecursoService>();
                endpoints.MapGrpcService<ImpedimentoTarefaService>();
                endpoints.MapGrpcService<RecursoProjetoService>();
                endpoints.MapGrpcService<RecursoTarefaService>();

                //endpoints.MapGrpcService<SistemaService>().EnableGrpcWeb();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
