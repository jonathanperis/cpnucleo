using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class ImpedimentoApiService : CrudApiService<ImpedimentoViewModel>, IImpedimentoApiService
    {
        private const string actionRoute = "impedimento";

        public bool Incluir(string token, ImpedimentoViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<ImpedimentoViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public ImpedimentoViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, ImpedimentoViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }
    }
}
