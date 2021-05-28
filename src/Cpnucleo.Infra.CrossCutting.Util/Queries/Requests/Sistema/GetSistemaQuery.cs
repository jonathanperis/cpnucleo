using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema
{
    public class GetSistemaQuery : IRequest<GetSistemaResponse>
    {
        public Guid Id { get; set; }
    }
}
