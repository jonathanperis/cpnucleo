using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class SistemaGrpcService : ISistemaGrpcService
    {
        private readonly IMediator _mediator;

        public SistemaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateSistemaResponse> AddAsync(CreateSistemaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListSistemaResponse> AllAsync(ListSistemaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetSistemaResponse> GetAsync(GetSistemaQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveSistemaResponse> RemoveAsync(RemoveSistemaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateSistemaResponse> UpdateAsync(UpdateSistemaCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
