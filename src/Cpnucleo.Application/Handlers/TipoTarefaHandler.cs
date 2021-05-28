using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class TipoTarefaHandler :
        IRequestHandler<CreateTipoTarefaComand, CreateTipoTarefaResponse>,
        IRequestHandler<GetTipoTarefaQuery, GetTipoTarefaResponse>,
        IRequestHandler<ListTipoTarefaQuery, ListTipoTarefaResponse>,
        IRequestHandler<RemoveTipoTarefaComand, RemoveTipoTarefaResponse>,
        IRequestHandler<UpdateTipoTarefaComand, UpdateTipoTarefaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateTipoTarefaResponse> Handle(CreateTipoTarefaComand request, CancellationToken cancellationToken)
        {
            CreateTipoTarefaResponse result = new CreateTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            TipoTarefa obj = await _unitOfWork.TipoTarefaRepository.AddAsync(_mapper.Map<TipoTarefa>(request.TipoTarefa));
            result.TipoTarefa = _mapper.Map<TipoTarefaViewModel>(obj);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetTipoTarefaResponse> Handle(GetTipoTarefaQuery request, CancellationToken cancellationToken)
        {
            GetTipoTarefaResponse result = new GetTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.TipoTarefa = _mapper.Map<TipoTarefaViewModel>(await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListTipoTarefaResponse> Handle(ListTipoTarefaQuery request, CancellationToken cancellationToken)
        {
            ListTipoTarefaResponse result = new ListTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.TipoTarefas = _mapper.Map<IEnumerable<TipoTarefaViewModel>>(await _unitOfWork.TipoTarefaRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveTipoTarefaResponse> Handle(RemoveTipoTarefaComand request, CancellationToken cancellationToken)
        {
            RemoveTipoTarefaResponse result = new RemoveTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            TipoTarefa obj = await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.TipoTarefaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateTipoTarefaResponse> Handle(UpdateTipoTarefaComand request, CancellationToken cancellationToken)
        {
            UpdateTipoTarefaResponse result = new UpdateTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.TipoTarefaRepository.Update(_mapper.Map<TipoTarefa>(request.TipoTarefa));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
