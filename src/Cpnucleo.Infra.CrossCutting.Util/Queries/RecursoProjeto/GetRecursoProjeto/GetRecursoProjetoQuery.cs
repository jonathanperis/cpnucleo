using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto
{
    [DataContract]
    public class GetRecursoProjetoQuery : IRequest<GetRecursoProjetoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
