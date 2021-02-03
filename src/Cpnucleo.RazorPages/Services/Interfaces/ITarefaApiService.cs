using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface ITarefaApiService : ICrudApiService<TarefaViewModel>
    {
        Task<IEnumerable<TarefaViewModel>> ListarPorRecursoAsync(string token, Guid idRecurso);

        Task<bool> AlterarPorWorkflowAsync(string token, Guid idTarefa, Guid idWorkflow);
    }
}
