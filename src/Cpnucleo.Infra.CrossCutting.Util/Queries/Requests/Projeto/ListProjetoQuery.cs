using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto
{
    [DataContract]
    public class ListProjetoQuery : IRequest<ListProjetoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
