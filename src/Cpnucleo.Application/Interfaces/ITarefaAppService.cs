namespace Cpnucleo.Application.Interfaces;

public interface ITarefaAppService : IGenericAppService<TarefaViewModel>
{
    Task<IEnumerable<TarefaViewModel>> GetByRecursoAsync(Guid idRecurso);
}
