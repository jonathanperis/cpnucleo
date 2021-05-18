using Cpnucleo.Domain.Commands.Requests.Projeto;
using Cpnucleo.Domain.Commands.Responses.Projeto;
using Cpnucleo.Domain.Queries.Requests.Projeto;
using Cpnucleo.Domain.Queries.Responses.Projeto;
using Cpnucleo.Domain.UoW;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class ProjetoHandler :
        IRequestHandler<CreateProjetoComand, CreateProjetoResponse>,
        IRequestHandler<GetProjetoQuery, GetProjetoResponse>,
        IRequestHandler<ListProjetoQuery, ListProjetoResponse>,
        IRequestHandler<RemoveProjetoComand, RemoveProjetoResponse>,
        IRequestHandler<UpdateProjetoComand, UpdateProjetoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjetoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProjetoResponse> Handle(CreateProjetoComand request, CancellationToken cancellationToken)
        {
            CreateProjetoResponse result = new CreateProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.Projeto = await _unitOfWork.ProjetoRepository.AddAsync(request.Projeto);

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

            result.Projeto = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListProjetoResponse> Handle(ListProjetoQuery request, CancellationToken cancellationToken)
        {
            ListProjetoResponse result = new ListProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.Projetos = await _unitOfWork.ProjetoRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveProjetoResponse> Handle(RemoveProjetoComand request, CancellationToken cancellationToken)
        {
            RemoveProjetoResponse result = new RemoveProjetoResponse
            {
                Status = OperationResult.Failed
            };

            Entities.Projeto obj = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.ProjetoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateProjetoResponse> Handle(UpdateProjetoComand request, CancellationToken cancellationToken)
        {
            UpdateProjetoResponse result = new UpdateProjetoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.ProjetoRepository.Update(request.Projeto);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
