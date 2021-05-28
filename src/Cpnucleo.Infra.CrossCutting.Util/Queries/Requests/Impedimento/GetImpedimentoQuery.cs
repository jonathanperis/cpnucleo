using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento
{
    public class GetImpedimentoQuery : IRequest<GetImpedimentoResponse>
    {
        public Guid Id { get; set; }
    }
}
