namespace Cpnucleo.Domain.Interfaces;

public interface IImpedimentoTarefaRepository : IGenericRepository<ImpedimentoTarefa>
{
    Task<IEnumerable<ImpedimentoTarefa>> GetImpedimentoTarefaByTarefaAsync(Guid idTarefa);
}
