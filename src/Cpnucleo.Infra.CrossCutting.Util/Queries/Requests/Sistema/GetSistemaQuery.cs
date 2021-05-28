using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema
{
    [DataContract]
    public class GetSistemaQuery : IRequest<GetSistemaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
