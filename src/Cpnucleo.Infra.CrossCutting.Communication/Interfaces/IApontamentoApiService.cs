using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Interfaces
{
    public interface IApontamentoApiService : ICrudApiService<ApontamentoViewModel>
    {
        IEnumerable<ApontamentoViewModel> ListarPorRecurso(string token, Guid id);
    }
}
