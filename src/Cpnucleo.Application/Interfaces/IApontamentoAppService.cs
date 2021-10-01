namespace Cpnucleo.Application.Interfaces;

public interface IApontamentoAppService : IGenericAppService<ApontamentoViewModel>
{
    Task<IEnumerable<ApontamentoViewModel>> GetByRecursoAsync(Guid idRecurso);
}
