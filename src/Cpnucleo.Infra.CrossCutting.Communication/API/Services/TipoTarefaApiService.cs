using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Services
{
    public class TipoTarefaApiService : CrudApiService<TipoTarefaViewModel>, ITipoTarefaApiService
    {
        private const string actionRoute = "tipoTarefa";

        public bool Incluir(string token, TipoTarefaViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<TipoTarefaViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public TipoTarefaViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, TipoTarefaViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }
    }
}
