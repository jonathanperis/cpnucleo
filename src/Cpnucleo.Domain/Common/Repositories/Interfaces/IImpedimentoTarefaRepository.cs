namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IImpedimentoTarefaRepository : IGenericRepository<ImpedimentoTarefa>
{
    IQueryable<ImpedimentoTarefa> GetImpedimentoTarefaByTarefa(Guid idTarefa);
}
