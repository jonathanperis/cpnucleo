using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class ProjetoApiService : CrudApiService<ProjetoViewModel>, IProjetoApiService
    {
        private const string actionRoute = "projeto";

        public bool Incluir(string token, ProjetoViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<ProjetoViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public ProjetoViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, ProjetoViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }
    }
}
