using Cpnucleo.Infrastructure.Context;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class ImpedimentoTarefaRepository : GenericRepository<ImpedimentoTarefa>, IImpedimentoTarefaRepository
{
    public ImpedimentoTarefaRepository(CpnucleoDbContext context)
        : base(context) { }

    public IQueryable<ImpedimentoTarefa> ListImpedimentoTarefaByTarefa(Guid idTarefa)
    {
        Expression<Func<ImpedimentoTarefa, bool>> predicate = x => x.IdTarefa == idTarefa && x.Ativo;

        return All(predicate, true);
    }
}
