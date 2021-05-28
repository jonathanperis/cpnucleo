using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema
{
    [DataContract]
    public class ListSistemaQuery : IRequest<ListSistemaResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
