using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa
{
    [DataContract]
    public class ListRecursoTarefaQuery : IRequest<ListRecursoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
