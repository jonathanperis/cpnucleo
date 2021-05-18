using Cpnucleo.Domain.Commands.Requests.Apontamento;
using Cpnucleo.Domain.Commands.Responses.Apontamento;
using Cpnucleo.Domain.Queries.Requests.Apontamento;
using Cpnucleo.Domain.Queries.Responses.Apontamento;
using Cpnucleo.Domain.UoW;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class ApontamentoHandler :
        IRequestHandler<CreateApontamentoComand, CreateApontamentoResponse>,
        IRequestHandler<GetApontamentoQuery, GetApontamentoResponse>,
        IRequestHandler<ListApontamentoQuery, ListApontamentoResponse>,
        IRequestHandler<RemoveApontamentoComand, RemoveApontamentoResponse>,
        IRequestHandler<UpdateApontamentoComand, UpdateApontamentoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApontamentoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateApontamentoResponse> Handle(CreateApontamentoComand request, CancellationToken cancellationToken)
        {
            CreateApontamentoResponse result = new CreateApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            result.Apontamento = await _unitOfWork.ApontamentoRepository.AddAsync(request.Apontamento);

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

            result.Apontamento = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListApontamentoResponse> Handle(ListApontamentoQuery request, CancellationToken cancellationToken)
        {
            ListApontamentoResponse result = new ListApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            result.Apontamentos = await _unitOfWork.ApontamentoRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveApontamentoResponse> Handle(RemoveApontamentoComand request, CancellationToken cancellationToken)
        {
            RemoveApontamentoResponse result = new RemoveApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            Entities.Apontamento obj = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.ApontamentoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateApontamentoResponse> Handle(UpdateApontamentoComand request, CancellationToken cancellationToken)
        {
            UpdateApontamentoResponse result = new UpdateApontamentoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.ApontamentoRepository.Update(request.Apontamento);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
