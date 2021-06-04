using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ImpedimentoTarefaGrpcService : ServiceBase<IImpedimentoTarefaGrpcService>, IImpedimentoTarefaGrpcService
    {
        private readonly IMediator _mediator;

        public ImpedimentoTarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateImpedimentoTarefaResponse> AddAsync(CreateImpedimentoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListImpedimentoTarefaResponse> AllAsync(ListImpedimentoTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetImpedimentoTarefaResponse> GetAsync(GetImpedimentoTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveImpedimentoTarefaResponse> RemoveAsync(RemoveImpedimentoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateImpedimentoTarefaResponse> UpdateAsync(UpdateImpedimentoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
