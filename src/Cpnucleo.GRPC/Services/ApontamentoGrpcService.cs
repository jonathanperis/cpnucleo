using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetTotalHorasPorRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ApontamentoGrpcService : IApontamentoGrpcService
    {
        private readonly IMediator _mediator;

        public ApontamentoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateApontamentoResponse> AddAsync(CreateApontamentoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListApontamentoResponse> AllAsync(ListApontamentoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetApontamentoResponse> GetAsync(GetApontamentoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetTotalHorasPorRecursoResponse> GetTotalHorasPorRecursoAsync(GetTotalHorasPorRecursoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveApontamentoResponse> RemoveAsync(RemoveApontamentoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateApontamentoResponse> UpdateAsync(UpdateApontamentoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
