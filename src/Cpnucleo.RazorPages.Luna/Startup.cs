using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Luna.Services.Api;
using Cpnucleo.RazorPages.Luna.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.RazorPages.Luna
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
            services
                .AddScoped<ICrudApiService<SistemaViewModel>, CrudApiService<SistemaViewModel>>()
                .AddScoped<ICrudApiService<ProjetoViewModel>, CrudApiService<ProjetoViewModel>>()
                .AddScoped<ICrudApiService<TarefaViewModel>, CrudApiService<TarefaViewModel>>()
                .AddScoped<ICrudApiService<ApontamentoViewModel>, CrudApiService<ApontamentoViewModel>>()
                .AddScoped<ICrudApiService<WorkflowViewModel>, CrudApiService<WorkflowViewModel>>()
                .AddScoped<ICrudApiService<RecursoViewModel>, CrudApiService<RecursoViewModel>>()
                .AddScoped<ICrudApiService<ImpedimentoViewModel>, CrudApiService<ImpedimentoViewModel>>()
                .AddScoped<ICrudApiService<ImpedimentoTarefaViewModel>, CrudApiService<ImpedimentoTarefaViewModel>>()
                .AddScoped<ICrudApiService<RecursoProjetoViewModel>, CrudApiService<RecursoProjetoViewModel>>()
                .AddScoped<ICrudApiService<RecursoTarefaViewModel>, CrudApiService<RecursoTarefaViewModel>>()
                .AddScoped<ICrudApiService<TipoTarefaViewModel>, CrudApiService<TipoTarefaViewModel>>();

            services
                .AddScoped<ISistemaApiService, SistemaApiService>()
                .AddScoped<IProjetoApiService, ProjetoApiService>()
                .AddScoped<ITarefaApiService, TarefaApiService>()
                .AddScoped<IApontamentoApiService, ApontamentoApiService>()
                .AddScoped<IWorkflowApiService, WorkflowApiService>()
                .AddScoped<IRecursoApiService, RecursoApiService>()
                .AddScoped<IImpedimentoApiService, ImpedimentoApiService>()
                .AddScoped<IImpedimentoTarefaApiService, ImpedimentoTarefaApiService>()
                .AddScoped<IRecursoProjetoApiService, RecursoProjetoApiService>()
                .AddScoped<IRecursoTarefaApiService, RecursoTarefaApiService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
