using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ImpedimentoGrpcService : IImpedimentoGrpcService
    {
        private readonly IMediator _mediator;

        public ImpedimentoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateImpedimentoResponse> AddAsync(CreateImpedimentoCommand command, CallContext context = default)
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

        public async Task<RemoveImpedimentoResponse> RemoveAsync(RemoveImpedimentoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateImpedimentoResponse> UpdateAsync(UpdateImpedimentoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
