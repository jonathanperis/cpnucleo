using Cpnucleo.Domain.Commands.Requests.ImpedimentoTarefa;
using Cpnucleo.Domain.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Domain.Queries.Requests.ImpedimentoTarefa;
using Cpnucleo.Domain.Queries.Responses.ImpedimentoTarefa;
using Cpnucleo.Domain.UoW;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class ImpedimentoTarefaHandler :
        IRequestHandler<CreateImpedimentoTarefaComand, CreateImpedimentoTarefaResponse>,
        IRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaResponse>,
        IRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaResponse>,
        IRequestHandler<RemoveImpedimentoTarefaComand, RemoveImpedimentoTarefaResponse>,
        IRequestHandler<UpdateImpedimentoTarefaComand, UpdateImpedimentoTarefaResponse>,
        IRequestHandler<GetByTarefaQuery, GetByTarefaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImpedimentoTarefaHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateImpedimentoTarefaResponse> Handle(CreateImpedimentoTarefaComand request, CancellationToken cancellationToken)
        {
            CreateImpedimentoTarefaResponse result = new CreateImpedimentoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.ImpedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(request.ImpedimentoTarefa);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetImpedimentoTarefaResponse> Handle(GetImpedimentoTarefaQuery request, CancellationToken cancellationToken)
        {
            GetImpedimentoTarefaResponse result = new GetImpedimentoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.ImpedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListImpedimentoTarefaResponse> Handle(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
        {
            ListImpedimentoTarefaResponse result = new ListImpedimentoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.ImpedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveImpedimentoTarefaResponse> Handle(RemoveImpedimentoTarefaComand request, CancellationToken cancellationToken)
        {
            RemoveImpedimentoTarefaResponse result = new RemoveImpedimentoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            Entities.ImpedimentoTarefa obj = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateImpedimentoTarefaResponse> Handle(UpdateImpedimentoTarefaComand request, CancellationToken cancellationToken)
        {
            UpdateImpedimentoTarefaResponse result = new UpdateImpedimentoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.ImpedimentoTarefaRepository.Update(request.ImpedimentoTarefa);

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

            result.ImpedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(request.IdTarefa);
            result.Status = OperationResult.Success;

            return result;
        }
    }
}
