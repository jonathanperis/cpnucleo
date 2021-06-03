using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto
{
    [DataContract]
    public class ListRecursoProjetoQuery : IRequest<ListRecursoProjetoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
