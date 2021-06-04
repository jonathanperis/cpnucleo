using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.GRPC.Services
{
    public class RecursoGrpcService : ServiceBase<IRecursoGrpcService>, IRecursoGrpcService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public RecursoGrpcService(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [Authorize]
        public async UnaryResult<CreateRecursoResponse> AddAsync(CreateRecursoCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        public async UnaryResult<ListRecursoResponse> AllAsync(ListRecursoQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<AuthResponse> AuthAsync(AuthQuery query)
        {
            AuthResponse response = await _mediator.Send(query);

            if (response.Status == OperationResult.Success)
            {
                int.TryParse(_configuration["Jwt:Expires"], out int jwtExpires);

                response.Recurso.Token = TokenService.GenerateToken(response.Recurso.Id.ToString(), _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);
            }

            return response;
        }

        [Authorize]
        public async UnaryResult<GetRecursoResponse> GetAsync(GetRecursoQuery query)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        public async UnaryResult<RemoveRecursoResponse> RemoveAsync(RemoveRecursoCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        public async UnaryResult<UpdateRecursoResponse> UpdateAsync(UpdateRecursoCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
