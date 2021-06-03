using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema
{
    [DataContract]
    public class GetSistemaQuery : IRequest<GetSistemaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
