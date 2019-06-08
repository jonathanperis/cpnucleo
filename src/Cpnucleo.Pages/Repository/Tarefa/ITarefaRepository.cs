namespace Cpnucleo.Pages.Repository.Tarefa
{
    public interface ITarefaRepository : IRepository<TarefaItem>
    {
        void AlterarPorFluxoTrabalho(int idTarefa, int idWorkflow);
    }
}