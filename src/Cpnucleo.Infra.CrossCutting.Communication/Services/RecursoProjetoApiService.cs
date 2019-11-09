using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class RecursoProjetoApiService : CrudApiService<RecursoProjetoViewModel>, IRecursoProjetoApiService
    {
        private const string actionRoute = "recursoProjeto";

        public bool Incluir(string token, RecursoProjetoViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<RecursoProjetoViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public RecursoProjetoViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, RecursoProjetoViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }

        public IEnumerable<RecursoProjetoViewModel> ListarPorProjeto(string token, Guid idProjeto)
        {
            throw new NotImplementedException();
        }
    }
}
