namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IApontamentoRepository : IGenericRepository<Apontamento>
{
    Task<int> GetTotalHorasByRecursoAsync(Guid idRecurso, Guid idTarefa);

    IQueryable<Apontamento> ListApontamentoByRecurso(Guid idRecurso);
}
