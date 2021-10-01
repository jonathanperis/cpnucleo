namespace Cpnucleo.Domain.Interfaces;

public interface IRecursoRepository : IGenericRepository<Recurso>
{
    Task<Recurso> GetByLoginAsync(string login);
}
