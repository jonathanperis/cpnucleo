using Cpnucleo.Domain.Commands.Requests.Impedimento;
using Cpnucleo.Domain.Commands.Responses.Impedimento;
using Cpnucleo.Domain.Queries.Requests.Impedimento;
using Cpnucleo.Domain.Queries.Responses.Impedimento;
using Cpnucleo.Domain.UoW;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class ImpedimentoHandler :
        IRequestHandler<CreateImpedimentoComand, CreateImpedimentoResponse>,
        IRequestHandler<GetImpedimentoQuery, GetImpedimentoResponse>,
        IRequestHandler<ListImpedimentoQuery, ListImpedimentoResponse>,
        IRequestHandler<RemoveImpedimentoComand, RemoveImpedimentoResponse>,
        IRequestHandler<UpdateImpedimentoComand, UpdateImpedimentoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImpedimentoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateImpedimentoResponse> Handle(CreateImpedimentoComand request, CancellationToken cancellationToken)
        {
            CreateImpedimentoResponse result = new CreateImpedimentoResponse
            {
                Status = OperationResult.Failed
            };

            result.Impedimento = await _unitOfWork.ImpedimentoRepository.AddAsync(request.Impedimento);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetImpedimentoResponse> Handle(GetImpedimentoQuery request, CancellationToken cancellationToken)
        {
            GetImpedimentoResponse result = new GetImpedimentoResponse
            {
                Status = OperationResult.Failed
            };

            result.Impedimento = await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListImpedimentoResponse> Handle(ListImpedimentoQuery request, CancellationToken cancellationToken)
        {
            ListImpedimentoResponse result = new ListImpedimentoResponse
            {
                Status = OperationResult.Failed
            };

            result.Impedimentos = await _unitOfWork.ImpedimentoRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveImpedimentoResponse> Handle(RemoveImpedimentoComand request, CancellationToken cancellationToken)
        {
            RemoveImpedimentoResponse result = new RemoveImpedimentoResponse
            {
                Status = OperationResult.Failed
            };

            Entities.Impedimento obj = await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.ImpedimentoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateImpedimentoResponse> Handle(UpdateImpedimentoComand request, CancellationToken cancellationToken)
        {
            UpdateImpedimentoResponse result = new UpdateImpedimentoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.ImpedimentoRepository.Update(request.Impedimento);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
