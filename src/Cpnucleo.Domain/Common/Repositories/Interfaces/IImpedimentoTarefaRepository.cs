namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IImpedimentoTarefaRepository : IGenericRepository<ImpedimentoTarefa>
{
    IQueryable<ImpedimentoTarefa> ListImpedimentoTarefaByTarefa(Guid idTarefa);
}
