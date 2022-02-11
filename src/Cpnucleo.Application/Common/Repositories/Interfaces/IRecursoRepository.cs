namespace Cpnucleo.Application.Common.Repositories.Interfaces;

public interface IRecursoRepository : IGenericRepository<Recurso>
{
    Task<Recurso> GetRecursoByLoginAsync(string login);
}
