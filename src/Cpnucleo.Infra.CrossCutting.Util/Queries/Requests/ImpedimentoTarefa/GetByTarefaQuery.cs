using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa
{
    public class GetByTarefaQuery : IRequest<GetByTarefaResponse>
    {
        public Guid IdTarefa { get; set; }
    }
}
