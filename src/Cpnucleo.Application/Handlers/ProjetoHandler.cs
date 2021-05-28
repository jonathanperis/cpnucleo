using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class ProjetoHandler :
        IRequestHandler<CreateProjetoCommand, CreateProjetoResponse>,
        IRequestHandler<GetProjetoQuery, GetProjetoResponse>,
        IRequestHandler<ListProjetoQuery, ListProjetoResponse>,
        IRequestHandler<RemoveProjetoCommand, RemoveProjetoResponse>,
        IRequestHandler<UpdateProjetoCommand, UpdateProjetoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateProjetoResponse> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
        {
            CreateProjetoResponse result = new CreateProjetoResponse
            {
                Status = OperationResult.Failed
            };

            Projeto obj = await _unitOfWork.ProjetoRepository.AddAsync(_mapper.Map<Projeto>(request.Projeto));
            result.Projeto = _mapper.Map<ProjetoViewModel>(obj);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetProjetoResponse> Handle(GetProjetoQuery request, CancellationToken cancellationToken)
        {
            GetProjetoResponse result = new GetProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.Projeto = _mapper.Map<ProjetoViewModel>(await _unitOfWork.ProjetoRepository.GetAsync(request.Id));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListProjetoResponse> Handle(ListProjetoQuery request, CancellationToken cancellationToken)
        {
            ListProjetoResponse result = new ListProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.Projetos = _mapper.Map<IEnumerable<ProjetoViewModel>>(await _unitOfWork.ProjetoRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveProjetoResponse> Handle(RemoveProjetoCommand request, CancellationToken cancellationToken)
        {
            RemoveProjetoResponse result = new RemoveProjetoResponse
            {
                Status = OperationResult.Failed
            };

            Projeto obj = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.ProjetoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateProjetoResponse> Handle(UpdateProjetoCommand request, CancellationToken cancellationToken)
        {
            UpdateProjetoResponse result = new UpdateProjetoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.ProjetoRepository.Update(_mapper.Map<Projeto>(request.Projeto));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
