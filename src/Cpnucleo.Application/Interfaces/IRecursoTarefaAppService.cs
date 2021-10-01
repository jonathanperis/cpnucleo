namespace Cpnucleo.Application.Interfaces;

public interface IRecursoTarefaAppService : IGenericAppService<RecursoTarefaViewModel>
{
    Task<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa);
}
