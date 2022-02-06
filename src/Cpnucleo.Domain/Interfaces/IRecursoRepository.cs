namespace Cpnucleo.Domain.Interfaces;

public interface IRecursoRepository : IGenericRepository<Recurso>
{
    Task<Recurso> GetRecursoByLoginAsync(string login);
}
