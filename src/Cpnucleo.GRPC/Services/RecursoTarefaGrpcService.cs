using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
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
