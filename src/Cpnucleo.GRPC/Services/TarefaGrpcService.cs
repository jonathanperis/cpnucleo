using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class TarefaGrpcService : ITarefaGrpcService
    {
        private readonly IMediator _mediator;

        public TarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateTarefaResponse> AddAsync(CreateTarefaCommand command, CallContext context = default)
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

        public async Task<RemoveTarefaResponse> RemoveAsync(RemoveTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateTarefaResponse> UpdateAsync(UpdateTarefaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
