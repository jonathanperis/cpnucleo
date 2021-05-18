using Cpnucleo.Domain.Queries.Responses.Impedimento;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Impedimento
{
    public class GetImpedimentoQuery : IRequest<GetImpedimentoResponse>
    {
        public Guid Id { get; set; }
    }
}
