using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Services
{
    public class WorkflowApiService : CrudApiService<WorkflowViewModel>, ICrudApiService<WorkflowViewModel>
    {
        private const string actionRoute = "workflow";

        public bool Incluir(string token, WorkflowViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<WorkflowViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public WorkflowViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, WorkflowViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }
    }
}
