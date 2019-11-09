using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class RecursoTarefaApiService : CrudApiService<RecursoTarefaViewModel>, IRecursoTarefaApiService
    {
        private const string actionRoute = "recursoTarefa";

        public bool Incluir(string token, RecursoTarefaViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<RecursoTarefaViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public RecursoTarefaViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, RecursoTarefaViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(string token, Guid idTarefa)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorRecurso(string token, Guid idRecurso)
        {
            throw new NotImplementedException();
        }
    }
}
