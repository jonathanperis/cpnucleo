using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTipoTarefa
{
    [DataContract]
    public class ListTipoTarefaQuery : IRequest<ListTipoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
