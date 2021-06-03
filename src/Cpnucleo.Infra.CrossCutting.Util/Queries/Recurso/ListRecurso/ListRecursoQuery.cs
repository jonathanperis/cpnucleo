using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso
{
    [DataContract]
    public class ListRecursoQuery : IRequest<ListRecursoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
