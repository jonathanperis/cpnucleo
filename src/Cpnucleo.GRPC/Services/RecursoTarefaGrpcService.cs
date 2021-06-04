using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class RecursoTarefaGrpcService : ServiceBase<IRecursoTarefaGrpcService>, IRecursoTarefaGrpcService
    {
        private readonly IMediator _mediator;

        public RecursoTarefaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateRecursoTarefaResponse> AddAsync(CreateRecursoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListRecursoTarefaResponse> AllAsync(ListRecursoTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetRecursoTarefaResponse> GetAsync(GetRecursoTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveRecursoTarefaResponse> RemoveAsync(RemoveRecursoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateRecursoTarefaResponse> UpdateAsync(UpdateRecursoTarefaCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
