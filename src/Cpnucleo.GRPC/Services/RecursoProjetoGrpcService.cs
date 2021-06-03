using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class RecursoProjetoGrpcService : IRecursoProjetoGrpcService
    {
        private readonly IMediator _mediator;

        public RecursoProjetoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateRecursoProjetoResponse> AddAsync(CreateRecursoProjetoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListRecursoProjetoResponse> AllAsync(ListRecursoProjetoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetRecursoProjetoResponse> GetAsync(GetRecursoProjetoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetByProjetoResponse> GetByProjetoAsync(GetByProjetoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveRecursoProjetoResponse> RemoveAsync(RemoveRecursoProjetoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateRecursoProjetoResponse> UpdateAsync(UpdateRecursoProjetoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
