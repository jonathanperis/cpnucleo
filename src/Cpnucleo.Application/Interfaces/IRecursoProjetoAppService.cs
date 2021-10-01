namespace Cpnucleo.Application.Interfaces;

public interface IRecursoProjetoAppService : IGenericAppService<RecursoProjetoViewModel>
{
    Task<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(Guid idProjeto);
}
