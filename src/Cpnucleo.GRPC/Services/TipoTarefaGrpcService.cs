using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class TipoTarefaGrpcService : ITipoTarefaGrpcService
    {
        private readonly IMediator _mediator;

        public TipoTarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateTipoTarefaResponse> AddAsync(CreateTipoTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListTipoTarefaResponse> AllAsync(ListTipoTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetTipoTarefaResponse> GetAsync(GetTipoTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveTipoTarefaResponse> RemoveAsync(RemoveTipoTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateTipoTarefaResponse> UpdateAsync(UpdateTipoTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
