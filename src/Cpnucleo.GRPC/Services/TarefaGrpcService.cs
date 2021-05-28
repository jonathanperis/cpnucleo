using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using MediatR;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class TarefaGrpcService : ITarefaGrpcService
    {
        private readonly IMediator _mediator;

        public TarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateTarefaResponse> AddAsync(CreateTarefaComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListTarefaResponse> AllAsync(ListTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetTarefaResponse> GetAsync(GetTarefaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveTarefaResponse> RemoveAsync(RemoveTarefaComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateTarefaResponse> UpdateAsync(UpdateTarefaComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
