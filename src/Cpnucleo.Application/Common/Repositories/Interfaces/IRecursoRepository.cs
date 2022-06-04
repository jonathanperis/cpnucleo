namespace Cpnucleo.Application.Common.Repositories.Interfaces;

public interface IRecursoRepository : IGenericRepository<Recurso>
{
    Task<Recurso> AddAsync(Recurso entity, string salt);

    Task<Recurso> GetRecursoByLoginAsync(string login);
}
