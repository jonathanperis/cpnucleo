using Cpnucleo.Infrastructure.Data.Context;

namespace Cpnucleo.Infrastructure.Data.Repositories;

internal class ApontamentoRepository : GenericRepository<Apontamento>, IApontamentoRepository
{
    public ApontamentoRepository(CpnucleoContext context)
        : base(context) { }

    public async Task<IEnumerable<Apontamento>> GetApontamentoByRecursoAsync(Guid idRecurso)
    {
        Expression<Func<Apontamento, bool>> predicate = x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.Now.AddDays(-30) && x.Ativo;

        return await All(predicate, true)
            .ToListAsync();
    }

    public async Task<int> GetTotalHorasByRecursoAsync(Guid idRecurso, Guid idTarefa)
    {
        Expression<Func<Apontamento, bool>> predicate = x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa && x.Ativo;

        return await All(predicate, true)
            .SumAsync(x => x.QtdHoras);
    }
}
