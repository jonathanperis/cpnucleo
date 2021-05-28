using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using MediatR;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class RecursoProjetoGrpcService : IRecursoProjetoGrpcService
    {
        private readonly IMediator _mediator;

        public RecursoProjetoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateRecursoProjetoResponse> AddAsync(CreateRecursoProjetoComand command, CallContext context = default)
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

        public async Task<RemoveRecursoProjetoResponse> RemoveAsync(RemoveRecursoProjetoComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateRecursoProjetoResponse> UpdateAsync(UpdateRecursoProjetoComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
