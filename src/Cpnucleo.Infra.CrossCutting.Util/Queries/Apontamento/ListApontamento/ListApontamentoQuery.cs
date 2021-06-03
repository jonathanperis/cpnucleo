using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento
{
    [DataContract]
    public class ListApontamentoQuery : IRequest<ListApontamentoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
