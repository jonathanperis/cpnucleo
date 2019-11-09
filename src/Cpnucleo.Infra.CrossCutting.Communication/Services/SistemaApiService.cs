using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class SistemaApiService : CrudApiService<SistemaViewModel>, ISistemaApiService
    {
        private const string actionRoute = "sistema";

        public bool Incluir(string token, SistemaViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<SistemaViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public SistemaViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, SistemaViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }
    }
}
