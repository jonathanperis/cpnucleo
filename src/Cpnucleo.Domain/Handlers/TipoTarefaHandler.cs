using Cpnucleo.Domain.Commands.Requests.TipoTarefa;
using Cpnucleo.Domain.Commands.Responses.TipoTarefa;
using Cpnucleo.Domain.Queries.Requests.TipoTarefa;
using Cpnucleo.Domain.Queries.Responses.TipoTarefa;
using Cpnucleo.Domain.UoW;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class TipoTarefaHandler :
        IRequestHandler<CreateTipoTarefaComand, CreateTipoTarefaResponse>,
        IRequestHandler<GetTipoTarefaQuery, GetTipoTarefaResponse>,
        IRequestHandler<ListTipoTarefaQuery, ListTipoTarefaResponse>,
        IRequestHandler<RemoveTipoTarefaComand, RemoveTipoTarefaResponse>,
        IRequestHandler<UpdateTipoTarefaComand, UpdateTipoTarefaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoTarefaHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTipoTarefaResponse> Handle(CreateTipoTarefaComand request, CancellationToken cancellationToken)
        {
            CreateTipoTarefaResponse result = new CreateTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.TipoTarefa = await _unitOfWork.TipoTarefaRepository.AddAsync(request.TipoTarefa);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetTipoTarefaResponse> Handle(GetTipoTarefaQuery request, CancellationToken cancellationToken)
        {
            GetTipoTarefaResponse result = new GetTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.TipoTarefa = await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListTipoTarefaResponse> Handle(ListTipoTarefaQuery request, CancellationToken cancellationToken)
        {
            ListTipoTarefaResponse result = new ListTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.TipoTarefas = await _unitOfWork.TipoTarefaRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveTipoTarefaResponse> Handle(RemoveTipoTarefaComand request, CancellationToken cancellationToken)
        {
            RemoveTipoTarefaResponse result = new RemoveTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            Entities.TipoTarefa obj = await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.TipoTarefaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateTipoTarefaResponse> Handle(UpdateTipoTarefaComand request, CancellationToken cancellationToken)
        {
            UpdateTipoTarefaResponse result = new UpdateTipoTarefaResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.TipoTarefaRepository.Update(request.TipoTarefa);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
