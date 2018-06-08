using dotnet_cpnucleo_pages.Configuration;
using dotnet_cpnucleo_pages.Hubs;
using dotnet_cpnucleo_pages.Repository;
using dotnet_cpnucleo_pages.Repository.Apontamento;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa;
using dotnet_cpnucleo_pages.Repository.Projeto;
using dotnet_cpnucleo_pages.Repository.Recurso;
using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using dotnet_cpnucleo_pages.Repository.RecursoTarefa;
using dotnet_cpnucleo_pages.Repository.Sistema;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using dotnet_cpnucleo_pages.Repository.TipoTarefa;
using dotnet_cpnucleo_pages.Repository.Workflow;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dotnet_cpnucleo_pages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Login", "");
            });

            services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

            services.Configure<ApplicationConfigurations>(
                Configuration.GetSection("ApplicationConfigurations"));

            services
                .AddDbContext<Context>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddScoped<IRepository<SistemaItem>, SistemaRepository>()
                .AddScoped<IRepository<ProjetoItem>, ProjetoRepository>()
                .AddScoped<IRepository<TarefaItem>, TarefaRepository>()
                .AddScoped<IRepository<ApontamentoItem>, ApontamentoRepository>()
                .AddScoped<IRepository<WorkflowItem>, WorkflowRepository>()
                .AddScoped<IRepository<RecursoItem>, RecursoRepository>()
                .AddScoped<IRepository<ImpedimentoItem>, ImpedimentoRepository>()
                .AddScoped<IRepository<ImpedimentoTarefaItem>, ImpedimentoTarefaRepository>()
                .AddScoped<IRepository<RecursoProjetoItem>, RecursoProjetoRepository>()
                .AddScoped<IRepository<RecursoTarefaItem>, RecursoTarefaRepository>()
                .AddScoped<IRepository<TipoTarefaItem>, TipoTarefaRepository>();

            services
                .AddScoped<ITarefaRepository, TarefaRepository>()
                .AddScoped<IApontamentoRepository, ApontamentoRepository>()
                .AddScoped<IWorkflowRepository, WorkflowRepository>()
                .AddScoped<IRecursoRepository, RecursoRepository>()
                .AddScoped<IImpedimentoTarefaRepository, ImpedimentoTarefaRepository>()
                .AddScoped<IRecursoProjetoRepository, RecursoProjetoRepository>()
                .AddScoped<IRecursoTarefaRepository, RecursoTarefaRepository>();

            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Login/");
                    options.AccessDeniedPath = new PathString("/Negado/");
                });

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {

            }
            else
            {
                app.UseExceptionHandler("/Erro");
                app.UseHsts();
                app.UseHttpsRedirection();                
            }

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<FluxoTrabalhoHub>("/hubs/fluxoTrabalho");
            });
        }
    }
}
