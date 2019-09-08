using Cpnucleo.Application.ViewModels;
using System;
using System.Linq;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoProjetoAppService : IAppService<RecursoProjetoViewModel>
    {
        IQueryable<RecursoProjetoViewModel> ListarPoridProjeto(Guid idProjeto);
    }
}
