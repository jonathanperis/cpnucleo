namespace Cpnucleo.Domain.Interfaces;

public interface IImpedimentoTarefaRepository : IGenericRepository<ImpedimentoTarefa>
{
    Task<IEnumerable<ImpedimentoTarefa>> GetByTarefaAsync(Guid idTarefa);
}
