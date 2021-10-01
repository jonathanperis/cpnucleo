namespace Cpnucleo.Domain.Interfaces;

public interface IRecursoProjetoRepository : IGenericRepository<RecursoProjeto>
{
    Task<IEnumerable<RecursoProjeto>> GetByProjetoAsync(Guid idProjeto);
}
