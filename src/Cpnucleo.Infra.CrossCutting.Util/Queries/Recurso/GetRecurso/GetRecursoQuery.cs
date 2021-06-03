using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso
{
    [DataContract]
    public class GetRecursoQuery : IRequest<GetRecursoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
