using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface IRecursoProjetoApiService : ICrudApiService<RecursoProjetoViewModel>
    {
        IEnumerable<RecursoProjetoViewModel> ListarPorProjeto(string token, Guid idProjeto);
    }
}
