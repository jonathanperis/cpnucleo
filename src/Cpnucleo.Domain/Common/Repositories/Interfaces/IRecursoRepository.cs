namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IRecursoRepository : IGenericRepository<Recurso>
{
    Task<Recurso> AddAsync(Recurso entity, string salt);

    IQueryable<Recurso> GetRecursoByLogin(string login);
}
