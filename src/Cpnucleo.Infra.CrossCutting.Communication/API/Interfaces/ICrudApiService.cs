using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface ICrudApiService<TViewModel>
    {
        bool Incluir(string token, TViewModel obj);

        TViewModel Consultar(string token, Guid id);

        IEnumerable<TViewModel> Listar(string token);

        bool Alterar(string token, TViewModel obj);

        bool Remover(string token, Guid id);
    }
}
