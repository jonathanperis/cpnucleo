using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class ProjetoGrpcService : ServiceBase<IProjetoGrpcService>, IProjetoGrpcService
    {
        private readonly IMediator _mediator;

        public ProjetoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateProjetoResponse> AddAsync(CreateProjetoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListProjetoResponse> AllAsync(ListProjetoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetProjetoResponse> GetAsync(GetProjetoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveProjetoResponse> RemoveAsync(RemoveProjetoCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateProjetoResponse> UpdateAsync(UpdateProjetoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
