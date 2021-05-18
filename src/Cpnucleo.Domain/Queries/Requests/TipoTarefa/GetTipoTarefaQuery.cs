using Cpnucleo.Domain.Queries.Responses.TipoTarefa;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.TipoTarefa
{
    public class GetTipoTarefaQuery : IRequest<GetTipoTarefaResponse>
    {
        public Guid Id { get; set; }
    }
}
