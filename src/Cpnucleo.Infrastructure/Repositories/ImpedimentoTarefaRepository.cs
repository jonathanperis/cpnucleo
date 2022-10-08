using Cpnucleo.Infrastructure.Context;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class ImpedimentoTarefaRepository : GenericRepository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
{
    public ImpedimentoTarefaRepository(CpnucleoContext context)
        : base(context) { }

    public async Task<IEnumerable<ImpedimentoTarefa>> GetImpedimentoTarefaByTarefaAsync(Guid idTarefa)
    {
        Expression<Func<ImpedimentoTarefa, bool>> predicate = x => x.IdTarefa == idTarefa && x.Ativo;

        return await All(predicate, true)
            .ToListAsync();
    }
}
