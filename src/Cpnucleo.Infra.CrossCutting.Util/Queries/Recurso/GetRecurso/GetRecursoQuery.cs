using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso
{
    [DataContract]
    public class GetRecursoQuery : IRequest<GetRecursoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
