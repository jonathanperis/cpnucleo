namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IRecursoProjetoRepository : IGenericRepository<RecursoProjeto>
{
    IQueryable<RecursoProjeto> GetRecursoProjetoByProjeto(Guid idProjeto);
}
