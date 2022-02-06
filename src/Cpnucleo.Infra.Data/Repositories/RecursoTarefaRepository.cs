namespace Cpnucleo.Infra.Data.Repositories;

internal class RecursoTarefaRepository : GenericRepository<RecursoTarefa>, IRecursoTarefaRepository
{
    public RecursoTarefaRepository(CpnucleoContext context)
        : base(context)
    {

    }

    public async Task<IEnumerable<RecursoTarefa>> GetRecursoTarefaByTarefaAsync(Guid idTarefa)
    {
        IEnumerable<RecursoTarefa> result = await AllAsync(true);

        return result
            .Where(x => x.IdTarefa == idTarefa)
            .ToList();
    }
}
