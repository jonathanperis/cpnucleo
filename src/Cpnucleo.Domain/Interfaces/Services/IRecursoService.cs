using Cpnucleo.Domain.Entities;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IRecursoService : ICrudService<Recurso>
    {
        Recurso Autenticar(string login, string senha, out bool valido);
    }
}
