using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using MediatR;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class ImpedimentoTarefaGrpcService : IImpedimentoTarefaGrpcService
    {
        private readonly IMediator _mediator;

        public ImpedimentoTarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateImpedimentoTarefaResponse> AddAsync(CreateImpedimentoTarefaComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListImpedimentoTarefaResponse> AllAsync(ListImpedimentoTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetImpedimentoTarefaResponse> GetAsync(GetImpedimentoTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveImpedimentoTarefaResponse> RemoveAsync(RemoveImpedimentoTarefaComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateImpedimentoTarefaResponse> UpdateAsync(UpdateImpedimentoTarefaComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
