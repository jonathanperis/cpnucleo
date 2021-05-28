using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
using MediatR;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class RecursoTarefaGrpcService : IRecursoTarefaGrpcService
    {
        private readonly IMediator _mediator;

        public RecursoTarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateRecursoTarefaResponse> AddAsync(CreateRecursoTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListRecursoTarefaResponse> AllAsync(ListRecursoTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetRecursoTarefaResponse> GetAsync(GetRecursoTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveRecursoTarefaResponse> RemoveAsync(RemoveRecursoTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateRecursoTarefaResponse> UpdateAsync(UpdateRecursoTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
