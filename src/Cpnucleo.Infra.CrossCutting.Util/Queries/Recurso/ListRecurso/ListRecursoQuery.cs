using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso
{
    [DataContract]
    public class ListRecursoQuery : IRequest<ListRecursoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
