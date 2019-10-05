using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoProjetoAppService : ICrudAppService<RecursoProjetoViewModel>
    {
        IEnumerable<RecursoProjetoViewModel> ListarPorProjeto(Guid idProjeto);
    }
}
