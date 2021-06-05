using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ApontamentoGrpcService : ServiceBase<IApontamentoGrpcService>, IApontamentoGrpcService
    {
        private readonly IMediator _mediator;

        public ApontamentoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateApontamentoResponse> AddAsync(CreateApontamentoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListApontamentoResponse> AllAsync(ListApontamentoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetApontamentoResponse> GetAsync(GetApontamentoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveApontamentoResponse> RemoveAsync(RemoveApontamentoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateApontamentoResponse> UpdateAsync(UpdateApontamentoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
