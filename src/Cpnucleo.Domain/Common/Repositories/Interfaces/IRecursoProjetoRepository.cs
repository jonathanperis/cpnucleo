namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IRecursoProjetoRepository : IGenericRepository<RecursoProjeto>
{
    Task<IEnumerable<RecursoProjeto>> GetRecursoProjetoByProjetoAsync(Guid idProjeto);
}
