using Cpnucleo.Pages.Models;

namespace Cpnucleo.Pages.Repository
{
    public interface ITarefaRepository : IRepository<TarefaItem>
    {
        void AlterarPorFluxoTrabalho(int idTarefa, int idWorkflow);
    }
}