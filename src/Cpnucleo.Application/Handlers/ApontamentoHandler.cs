﻿using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Handlers
{
    public class ApontamentoHandler :
        IRequestHandler<CreateApontamentoCommand, CreateApontamentoResponse>,
        IRequestHandler<GetApontamentoQuery, GetApontamentoResponse>,
        IRequestHandler<ListApontamentoQuery, ListApontamentoResponse>,
        IRequestHandler<RemoveApontamentoCommand, RemoveApontamentoResponse>,
        IRequestHandler<UpdateApontamentoCommand, UpdateApontamentoResponse>,
        IRequestHandler<GetByRecursoQuery, GetByRecursoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApontamentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateApontamentoResponse> Handle(CreateApontamentoCommand request, CancellationToken cancellationToken)
        {
            CreateApontamentoResponse result = new CreateApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            Apontamento obj = await _unitOfWork.ApontamentoRepository.AddAsync(_mapper.Map<Apontamento>(request.Apontamento));
            result.Apontamento = _mapper.Map<ApontamentoViewModel>(obj);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetApontamentoResponse> Handle(GetApontamentoQuery request, CancellationToken cancellationToken)
        {
            GetApontamentoResponse result = new GetApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            result.Apontamento = _mapper.Map<ApontamentoViewModel>(await _unitOfWork.ApontamentoRepository.GetAsync(request.Id));

            if (result.Apontamento == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListApontamentoResponse> Handle(ListApontamentoQuery request, CancellationToken cancellationToken)
        {
            ListApontamentoResponse result = new ListApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            result.Apontamentos = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveApontamentoResponse> Handle(RemoveApontamentoCommand request, CancellationToken cancellationToken)
        {
            RemoveApontamentoResponse result = new RemoveApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            Apontamento obj = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            await _unitOfWork.ApontamentoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateApontamentoResponse> Handle(UpdateApontamentoCommand request, CancellationToken cancellationToken)
        {
            UpdateApontamentoResponse result = new UpdateApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.ApontamentoRepository.Update(_mapper.Map<Apontamento>(request.Apontamento));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<GetByRecursoResponse> Handle(GetByRecursoQuery request, CancellationToken cancellationToken)
        {
            GetByRecursoResponse result = new GetByRecursoResponse
            {
                Status = OperationResult.Failed
            };

            result.Apontamentos = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(request.IdRecurso));
            result.Status = OperationResult.Success;

            return result;
        }
    }
}
