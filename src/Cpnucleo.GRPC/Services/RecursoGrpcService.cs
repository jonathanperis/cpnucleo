using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using MediatR;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class RecursoGrpcService : IRecursoGrpcService
    {
        private readonly IMediator _mediator;

        public RecursoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateRecursoResponse> AddAsync(CreateRecursoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListRecursoResponse> AllAsync(ListRecursoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetRecursoResponse> GetAsync(GetRecursoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveRecursoResponse> RemoveAsync(RemoveRecursoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateRecursoResponse> UpdateAsync(UpdateRecursoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
