namespace Cpnucleo.Application.Interfaces;

public interface IImpedimentoTarefaAppService : IGenericAppService<ImpedimentoTarefaViewModel>
{
    Task<IEnumerable<ImpedimentoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa);
}
