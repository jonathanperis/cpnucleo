using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class RecursoProjetoHandler :
        IRequestHandler<CreateRecursoProjetoCommand, CreateRecursoProjetoResponse>,
        IRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoResponse>,
        IRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoResponse>,
        IRequestHandler<RemoveRecursoProjetoCommand, RemoveRecursoProjetoResponse>,
        IRequestHandler<UpdateRecursoProjetoCommand, UpdateRecursoProjetoResponse>,
        IRequestHandler<GetByProjetoQuery, GetByProjetoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecursoProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateRecursoProjetoResponse> Handle(CreateRecursoProjetoCommand request, CancellationToken cancellationToken)
        {
            CreateRecursoProjetoResponse result = new CreateRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            RecursoProjeto obj = await _unitOfWork.RecursoProjetoRepository.AddAsync(_mapper.Map<RecursoProjeto>(request.RecursoProjeto));
            result.RecursoProjeto = _mapper.Map<RecursoProjetoViewModel>(obj);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetRecursoProjetoResponse> Handle(GetRecursoProjetoQuery request, CancellationToken cancellationToken)
        {
            GetRecursoProjetoResponse result = new GetRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoProjeto = _mapper.Map<RecursoProjetoViewModel>(await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListRecursoProjetoResponse> Handle(ListRecursoProjetoQuery request, CancellationToken cancellationToken)
        {
            ListRecursoProjetoResponse result = new ListRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoProjetos = _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveRecursoProjetoResponse> Handle(RemoveRecursoProjetoCommand request, CancellationToken cancellationToken)
        {
            RemoveRecursoProjetoResponse result = new RemoveRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            RecursoProjeto obj = await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.RecursoProjetoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateRecursoProjetoResponse> Handle(UpdateRecursoProjetoCommand request, CancellationToken cancellationToken)
        {
            UpdateRecursoProjetoResponse result = new UpdateRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.RecursoProjetoRepository.Update(_mapper.Map<RecursoProjeto>(request.RecursoProjeto));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<GetByProjetoResponse> Handle(GetByProjetoQuery request, CancellationToken cancellationToken)
        {
            GetByProjetoResponse result = new GetByProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoProjetos = _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(request.IdProjeto));
            result.Status = OperationResult.Success;

            return result;
        }
    }
}
