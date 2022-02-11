namespace Cpnucleo.Application.Common.Repositories.Interfaces;

public interface IImpedimentoTarefaRepository : IGenericRepository<ImpedimentoTarefa>
{
    Task<IEnumerable<ImpedimentoTarefa>> GetImpedimentoTarefaByTarefaAsync(Guid idTarefa);
}
