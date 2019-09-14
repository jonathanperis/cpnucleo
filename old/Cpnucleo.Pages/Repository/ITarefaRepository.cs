using Cpnucleo.Pages.Models;

namespace Cpnucleo.Pages.Repository
{
    public interface ITarefaRepository : IRepository<TarefaModel>
    {
        void AlterarPorFluxoTrabalho(int idTarefa, int idWorkflow);
    }
}