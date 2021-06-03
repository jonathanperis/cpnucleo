using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ProjetoGrpcService : IProjetoGrpcService
    {
        private readonly IMediator _mediator;

        public ProjetoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateProjetoResponse> AddAsync(CreateProjetoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListProjetoResponse> AllAsync(ListProjetoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetProjetoResponse> GetAsync(GetProjetoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveProjetoResponse> RemoveAsync(RemoveProjetoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateProjetoResponse> UpdateAsync(UpdateProjetoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
