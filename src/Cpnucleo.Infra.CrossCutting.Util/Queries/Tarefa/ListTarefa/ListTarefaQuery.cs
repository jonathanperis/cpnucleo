using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa
{
    [DataContract]
    public class ListTarefaQuery : IRequest<ListTarefaResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
