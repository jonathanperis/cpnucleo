using Cpnucleo.Domain.Queries.Responses.ImpedimentoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.ImpedimentoTarefa
{
    public class GetImpedimentoTarefaQuery : IRequest<GetImpedimentoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
