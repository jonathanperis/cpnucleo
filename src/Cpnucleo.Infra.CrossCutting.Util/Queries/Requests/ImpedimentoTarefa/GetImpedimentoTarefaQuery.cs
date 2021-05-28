using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa
{
    public class GetImpedimentoTarefaQuery : IRequest<GetImpedimentoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
