using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento
{
    [DataContract]
    public class GetTotalHorasPorRecursoQuery : IRequest<GetTotalHorasPorRecursoResponse>
    {
        [DataMember(Order = 1)]
        public Guid IdRecurso { get; set; }

        [DataMember(Order = 2)]
        public Guid IdTarefa { get; set; }
    }
}
