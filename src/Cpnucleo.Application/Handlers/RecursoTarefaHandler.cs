using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class RecursoTarefaHandler :
        IRequestHandler<CreateRecursoTarefaComand, CreateRecursoTarefaResponse>,
        IRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaResponse>,
        IRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaResponse>,
        IRequestHandler<RemoveRecursoTarefaComand, RemoveRecursoTarefaResponse>,
        IRequestHandler<UpdateRecursoTarefaComand, UpdateRecursoTarefaResponse>,
        IRequestHandler<GetByTarefaQuery, GetByTarefaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecursoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateRecursoTarefaResponse> Handle(CreateRecursoTarefaComand request, CancellationToken cancellationToken)
        {
            CreateRecursoTarefaResponse result = new CreateRecursoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            RecursoTarefa obj = await _unitOfWork.RecursoTarefaRepository.AddAsync(_mapper.Map<RecursoTarefa>(request.RecursoTarefa));
            result.RecursoTarefa = _mapper.Map<RecursoTarefaViewModel>(obj);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetRecursoTarefaResponse> Handle(GetRecursoTarefaQuery request, CancellationToken cancellationToken)
        {
            GetRecursoTarefaResponse result = new GetRecursoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoTarefa = _mapper.Map<RecursoTarefaViewModel>(await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListRecursoTarefaResponse> Handle(ListRecursoTarefaQuery request, CancellationToken cancellationToken)
        {
            ListRecursoTarefaResponse result = new ListRecursoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoTarefas = _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveRecursoTarefaResponse> Handle(RemoveRecursoTarefaComand request, CancellationToken cancellationToken)
        {
            RemoveRecursoTarefaResponse result = new RemoveRecursoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            RecursoTarefa obj = await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.RecursoTarefaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateRecursoTarefaResponse> Handle(UpdateRecursoTarefaComand request, CancellationToken cancellationToken)
        {
            UpdateRecursoTarefaResponse result = new UpdateRecursoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.RecursoTarefaRepository.Update(_mapper.Map<RecursoTarefa>(request.RecursoTarefa));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<GetByTarefaResponse> Handle(GetByTarefaQuery request, CancellationToken cancellationToken)
        {
            GetByTarefaResponse result = new GetByTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoTarefas = _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(request.IdTarefa));
            result.Status = OperationResult.Success;

            return result;
        }
    }
}
