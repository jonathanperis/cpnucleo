using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa
{
    public class GetTipoTarefaQuery : IRequest<GetTipoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
