using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class TipoTarefaGrpcService : ServiceBase<ITipoTarefaGrpcService>, ITipoTarefaGrpcService
    {
        private readonly IMediator _mediator;

        public TipoTarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateTipoTarefaResponse> AddAsync(CreateTipoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListTipoTarefaResponse> AllAsync(ListTipoTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetTipoTarefaResponse> GetAsync(GetTipoTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveTipoTarefaResponse> RemoveAsync(RemoveTipoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateTipoTarefaResponse> UpdateAsync(UpdateTipoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
