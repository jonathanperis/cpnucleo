using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class RecursoGrpcService : IRecursoGrpcService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public RecursoGrpcService(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [Authorize]
        public async Task<CreateRecursoResponse> AddAsync(CreateRecursoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        public async Task<ListRecursoResponse> AllAsync(ListRecursoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<AuthResponse> AuthAsync(AuthQuery query, CallContext context = default)
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
        public async Task<GetRecursoResponse> GetAsync(GetRecursoQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        public async Task<RemoveRecursoResponse> RemoveAsync(RemoveRecursoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        public async Task<UpdateRecursoResponse> UpdateAsync(UpdateRecursoCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
