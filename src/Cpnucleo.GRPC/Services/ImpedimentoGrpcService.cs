using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using MediatR;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class ImpedimentoGrpcService : IImpedimentoGrpcService
    {
        private readonly IMediator _mediator;

        public ImpedimentoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateImpedimentoResponse> AddAsync(CreateImpedimentoComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListImpedimentoResponse> AllAsync(ListImpedimentoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetImpedimentoResponse> GetAsync(GetImpedimentoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveImpedimentoResponse> RemoveAsync(RemoveImpedimentoComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateImpedimentoResponse> UpdateAsync(UpdateImpedimentoComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
