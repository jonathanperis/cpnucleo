namespace Cpnucleo.Domain.Interfaces;

public interface IApontamentoRepository : IGenericRepository<Apontamento>
{
    Task<int> GetTotalHorasByRecursoAsync(Guid idRecurso, Guid idTarefa);

    Task<IEnumerable<Apontamento>> GetApontamentoByRecursoAsync(Guid idRecurso);
}
