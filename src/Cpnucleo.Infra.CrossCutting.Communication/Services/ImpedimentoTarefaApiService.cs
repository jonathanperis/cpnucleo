using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class ImpedimentoTarefaApiService : CrudApiService<ImpedimentoTarefaViewModel>, IImpedimentoTarefaApiService
    {
        private const string actionRoute = "impedimentoTarefa";

        public bool Incluir(string token, ImpedimentoTarefaViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<ImpedimentoTarefaViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public ImpedimentoTarefaViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, ImpedimentoTarefaViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }

        public IEnumerable<ImpedimentoTarefaViewModel> ListarPorTarefa(string token, Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}
