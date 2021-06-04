using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class TarefaGrpcService : ServiceBase<ITarefaGrpcService>, ITarefaGrpcService
    {
        private readonly IMediator _mediator;

        public TarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateTarefaResponse> AddAsync(CreateTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListTarefaResponse> AllAsync(ListTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetTarefaResponse> GetAsync(GetTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveTarefaResponse> RemoveAsync(RemoveTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateTarefaResponse> UpdateAsync(UpdateTarefaCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
