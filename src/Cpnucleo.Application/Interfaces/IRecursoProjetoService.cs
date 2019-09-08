using Cpnucleo.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoProjetoAppService : IAppService<RecursoProjetoViewModel>
    {
        IEnumerable<RecursoProjetoViewModel> ListarPoridProjeto(Guid idProjeto);
    }
}
