using Cpnucleo.Infrastructure.Context;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class ApontamentoRepository : GenericRepository<Apontamento>, IApontamentoRepository
{
    public ApontamentoRepository(CpnucleoDbContext context)
        : base(context) { }

    public async Task<IEnumerable<Apontamento>> GetApontamentoByRecursoAsync(Guid idRecurso)
    {
        Expression<Func<Apontamento, bool>> predicate = x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.UtcNow.AddDays(-30) && x.Ativo;

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
