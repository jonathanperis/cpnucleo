namespace Cpnucleo.Infra.Data.Repositories;

internal class ApontamentoRepository : GenericRepository<Apontamento>, IApontamentoRepository
{
    public ApontamentoRepository(CpnucleoContext context)
        : base(context)
    {

    }

    public async Task<IEnumerable<Apontamento>> GetApontamentoByRecursoAsync(Guid idRecurso)
    {
        IEnumerable<Apontamento> result = await AllAsync(true);

        return result
            .Where(x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.Now.AddDays(-30))
            .ToList();
    }

    public async Task<int> GetTotalHorasByRecursoAsync(Guid idRecurso, Guid idTarefa)
    {
        IEnumerable<Apontamento> result = await AllAsync(true);

        return result
            .Where(x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa)
            .Sum(x => x.QtdHoras);
    }
}
