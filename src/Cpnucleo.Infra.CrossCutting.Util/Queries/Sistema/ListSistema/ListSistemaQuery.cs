using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema
{
    [DataContract]
    public class ListSistemaQuery : IRequest<ListSistemaResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
