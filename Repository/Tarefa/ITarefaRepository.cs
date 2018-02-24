namespace dotnet_cpnucleo_pages.Repository.Tarefa
{
    public interface ITarefaRepository : IRepository<TarefaItem>
    {
        void AlterarPorFluxoTrabalho(int idTarefa, int idWorkflow);
    }
}