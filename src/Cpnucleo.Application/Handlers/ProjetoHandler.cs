using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MessagePipe;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Handlers
{
    public class ProjetoHandler :
        IAsyncRequestHandler<CreateProjetoCommand, CreateProjetoResponse>,
        IAsyncRequestHandler<GetProjetoQuery, GetProjetoResponse>,
        IAsyncRequestHandler<ListProjetoQuery, ListProjetoResponse>,
        IAsyncRequestHandler<RemoveProjetoCommand, RemoveProjetoResponse>,
        IAsyncRequestHandler<UpdateProjetoCommand, UpdateProjetoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<CreateProjetoResponse> InvokeAsync(CreateProjetoCommand request, CancellationToken cancellationToken)
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

        public async ValueTask<GetProjetoResponse> InvokeAsync(GetProjetoQuery request, CancellationToken cancellationToken)
        {
            GetProjetoResponse result = new GetProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.Projeto = _mapper.Map<ProjetoViewModel>(await _unitOfWork.ProjetoRepository.GetAsync(request.Id));

            if (result.Projeto == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<ListProjetoResponse> InvokeAsync(ListProjetoQuery request, CancellationToken cancellationToken)
        {
            ListProjetoResponse result = new ListProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.Projetos = _mapper.Map<IEnumerable<ProjetoViewModel>>(await _unitOfWork.ProjetoRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<RemoveProjetoResponse> InvokeAsync(RemoveProjetoCommand request, CancellationToken cancellationToken)
        {
            RemoveProjetoResponse result = new RemoveProjetoResponse
            {
                Status = OperationResult.Failed
            };

            Projeto obj = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            await _unitOfWork.ProjetoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async ValueTask<UpdateProjetoResponse> InvokeAsync(UpdateProjetoCommand request, CancellationToken cancellationToken)
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
