using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MessagePipe;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Handlers
{
    public class TipoTarefaHandler :
        IAsyncRequestHandler<CreateTipoTarefaCommand, CreateTipoTarefaResponse>,
        IAsyncRequestHandler<GetTipoTarefaQuery, GetTipoTarefaResponse>,
        IAsyncRequestHandler<ListTipoTarefaQuery, ListTipoTarefaResponse>,
        IAsyncRequestHandler<RemoveTipoTarefaCommand, RemoveTipoTarefaResponse>,
        IAsyncRequestHandler<UpdateTipoTarefaCommand, UpdateTipoTarefaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<CreateTipoTarefaResponse> InvokeAsync(CreateTipoTarefaCommand request, CancellationToken cancellationToken)
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

        public async ValueTask<GetTipoTarefaResponse> InvokeAsync(GetTipoTarefaQuery request, CancellationToken cancellationToken)
        {
            GetTipoTarefaResponse result = new GetTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.TipoTarefa = _mapper.Map<TipoTarefaViewModel>(await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id));

            if (result.TipoTarefa == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<ListTipoTarefaResponse> InvokeAsync(ListTipoTarefaQuery request, CancellationToken cancellationToken)
        {
            ListTipoTarefaResponse result = new ListTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.TipoTarefas = _mapper.Map<IEnumerable<TipoTarefaViewModel>>(await _unitOfWork.TipoTarefaRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<RemoveTipoTarefaResponse> InvokeAsync(RemoveTipoTarefaCommand request, CancellationToken cancellationToken)
        {
            RemoveTipoTarefaResponse result = new RemoveTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            TipoTarefa obj = await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            await _unitOfWork.TipoTarefaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async ValueTask<UpdateTipoTarefaResponse> InvokeAsync(UpdateTipoTarefaCommand request, CancellationToken cancellationToken)
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
