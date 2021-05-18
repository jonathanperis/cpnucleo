using Cpnucleo.Domain.Queries.Responses.ImpedimentoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.ImpedimentoTarefa
{
    public class GetByTarefaQuery : IRequest<GetByTarefaResponse>
    {
        public Guid IdTarefa { get; set; }
    }
}
