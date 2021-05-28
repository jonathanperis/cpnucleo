using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.RecursoProto;
using Cpnucleo.Domain.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Microsoft.Extensions.Configuration;
using Cpnucleo.GRPC.Services;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC
{
    public class RecursoService : RecursoProto.RecursoProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyManager _cryptographyManager;
        private readonly IConfiguration _configuration;

        public RecursoService(IMapper mapper, 
                                    IUnitOfWork unitOfWork,
                                    ICryptographyManager cryptographyManager,
                                    IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cryptographyManager = cryptographyManager;
            _configuration = configuration;            
        }

        [Authorize]
        public override async Task<RecursoModel> Incluir(RecursoModel request, ServerCallContext context)
        {
            RecursoModel result = _mapper.Map<RecursoModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        [Authorize]
        public override async Task Listar(Empty request, IServerStreamWriter<RecursoModel> responseStream, ServerCallContext context)
        {
            foreach (RecursoModel item in _mapper.Map<IEnumerable<RecursoModel>>(_unitOfWork.RecursoRepository.AllAsync()))
            {
                await responseStream.WriteAsync(item);
            }
        }

        [Authorize]
        public override async Task<RecursoModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            RecursoModel result = _mapper.Map<RecursoModel>(_unitOfWork.RecursoRepository.GetAsync(id));

            return await Task.FromResult(result);
        }

        [Authorize]
        public override async Task<BaseReply> Alterar(RecursoModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(request))
            });
        }

        [Authorize]
        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.RecursoRepository.Remove(new Guid(request.Id))
            });
        }

        public override async Task<RecursoModel> Autenticar(AutenticarRequest request, ServerCallContext context)
        {
            bool valido = false;

            RecursoModel recurso = _mapper.Map<RecursoModel>(_unitOfWork.RecursoRepository.GetByLoginAsync(request.Login));

            if (recurso == null)
            {
                //@@JONATHAN - ESTUDAR UMA MANEIRA DE DEVOLVER UM VALOR NULO VÁLIDO ATRAVÉS DO GRPC.
            }

            valido = _cryptographyManager.VerifyPbkdf2(request.Senha, recurso.Senha, recurso.Salt);

            recurso.Senha = null;
            recurso.Salt = null;

            if (!valido)
            {
                //@@JONATHAN - ESTUDAR UMA MANEIRA DE DEVOLVER UM VALOR NULO VÁLIDO ATRAVÉS DO GRPC.
            }
            else
            {
                int jwtExpires;
                int.TryParse(_configuration["Jwt:Expires"], out jwtExpires);

                recurso.Token = TokenService.GenerateToken(recurso.Id.ToString(), _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);
            }

            return await Task.FromResult(recurso);
        }
    }
}
