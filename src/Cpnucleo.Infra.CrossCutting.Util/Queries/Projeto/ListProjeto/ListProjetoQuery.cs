using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto
{
    [DataContract]
    public class ListProjetoQuery : IRequest<ListProjetoResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
