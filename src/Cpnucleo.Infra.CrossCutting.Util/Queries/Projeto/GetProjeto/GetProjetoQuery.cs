using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto
{
    [DataContract]
    public class GetProjetoQuery : IRequest<GetProjetoResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
