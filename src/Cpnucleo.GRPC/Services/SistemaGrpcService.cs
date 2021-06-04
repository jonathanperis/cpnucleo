using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class SistemaGrpcService : ServiceBase<ISistemaGrpcService>, ISistemaGrpcService
    {
        private readonly IMediator _mediator;

        public SistemaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateSistemaResponse> AddAsync(CreateSistemaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListSistemaResponse> AllAsync(ListSistemaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetSistemaResponse> GetAsync(GetSistemaQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveSistemaResponse> RemoveAsync(RemoveSistemaCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateSistemaResponse> UpdateAsync(UpdateSistemaCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
