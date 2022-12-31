using Cpnucleo.Infrastructure.Context;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class ApontamentoRepository : GenericRepository<Apontamento>, IApontamentoRepository
{
    public ApontamentoRepository(CpnucleoDbContext context)
        : base(context) { }

    public IQueryable<Apontamento> ListApontamentoByRecurso(Guid idRecurso)
    {
        Expression<Func<Apontamento, bool>> predicate = x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.UtcNow.AddDays(-30) && x.Ativo;

        return All(predicate, true);
    }

    public async Task<int> GetTotalHorasByRecursoAsync(Guid idRecurso, Guid idTarefa)
    {
        Expression<Func<Apontamento, bool>> predicate = x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa && x.Ativo;

        return await All(predicate, true)
            .SumAsync(x => x.QtdHoras);
    }
}
