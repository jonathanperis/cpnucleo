using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ImpedimentoGrpcService : ServiceBase<IImpedimentoGrpcService>, IImpedimentoGrpcService
    {
        private readonly IMediator _mediator;

        public ImpedimentoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateImpedimentoResponse> AddAsync(CreateImpedimentoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListImpedimentoResponse> AllAsync(ListImpedimentoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetImpedimentoResponse> GetAsync(GetImpedimentoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveImpedimentoResponse> RemoveAsync(RemoveImpedimentoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateImpedimentoResponse> UpdateAsync(UpdateImpedimentoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
