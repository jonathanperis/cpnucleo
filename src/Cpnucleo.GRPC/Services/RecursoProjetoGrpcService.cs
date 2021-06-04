using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class RecursoProjetoGrpcService : ServiceBase<IRecursoProjetoGrpcService>, IRecursoProjetoGrpcService
    {
        private readonly IMediator _mediator;

        public RecursoProjetoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateRecursoProjetoResponse> AddAsync(CreateRecursoProjetoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListRecursoProjetoResponse> AllAsync(ListRecursoProjetoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetRecursoProjetoResponse> GetAsync(GetRecursoProjetoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetByProjetoResponse> GetByProjetoAsync(GetByProjetoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveRecursoProjetoResponse> RemoveAsync(RemoveRecursoProjetoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateRecursoProjetoResponse> UpdateAsync(UpdateRecursoProjetoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
