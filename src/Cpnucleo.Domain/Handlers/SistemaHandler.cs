using Cpnucleo.Domain.Commands.Requests.Sistema;
using Cpnucleo.Domain.Commands.Responses.Sistema;
using Cpnucleo.Domain.Queries.Requests.Sistema;
using Cpnucleo.Domain.Queries.Responses.Sistema;
using Cpnucleo.Domain.UoW;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class SistemaHandler :
        IRequestHandler<CreateSistemaComand, CreateSistemaResponse>,
        IRequestHandler<GetSistemaQuery, GetSistemaResponse>,
        IRequestHandler<ListSistemaQuery, ListSistemaResponse>,
        IRequestHandler<RemoveSistemaComand, RemoveSistemaResponse>,
        IRequestHandler<UpdateSistemaComand, UpdateSistemaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SistemaHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSistemaResponse> Handle(CreateSistemaComand request, CancellationToken cancellationToken)
        {
            CreateSistemaResponse result = new CreateSistemaResponse
            {
                Status = OperationResult.Failed
            };

            result.Sistema = await _unitOfWork.SistemaRepository.AddAsync(request.Sistema);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetSistemaResponse> Handle(GetSistemaQuery request, CancellationToken cancellationToken)
        {
            GetSistemaResponse result = new GetSistemaResponse
            {
                Status = OperationResult.Failed
            };

            result.Sistema = await _unitOfWork.SistemaRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListSistemaResponse> Handle(ListSistemaQuery request, CancellationToken cancellationToken)
        {
            ListSistemaResponse result = new ListSistemaResponse
            {
                Status = OperationResult.Failed
            };

            result.Sistemas = await _unitOfWork.SistemaRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveSistemaResponse> Handle(RemoveSistemaComand request, CancellationToken cancellationToken)
        {
            RemoveSistemaResponse result = new RemoveSistemaResponse
            {
                Status = OperationResult.Failed
            };

            Entities.Sistema obj = await _unitOfWork.SistemaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.SistemaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateSistemaResponse> Handle(UpdateSistemaComand request, CancellationToken cancellationToken)
        {
            UpdateSistemaResponse result = new UpdateSistemaResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.SistemaRepository.Update(request.Sistema);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
