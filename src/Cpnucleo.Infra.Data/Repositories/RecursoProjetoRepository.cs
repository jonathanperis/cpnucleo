namespace Cpnucleo.Infra.Data.Repositories;

internal class RecursoProjetoRepository : GenericRepository<RecursoProjeto>, IRecursoProjetoRepository
{
    public RecursoProjetoRepository(CpnucleoContext context)
        : base(context)
    {

    }

    public async Task<IEnumerable<RecursoProjeto>> GetByProjetoAsync(Guid idProjeto)
    {
        IEnumerable<RecursoProjeto> result = await AllAsync(true);

        return result
            .Where(x => x.IdProjeto == idProjeto)
            .ToList();
    }
}
