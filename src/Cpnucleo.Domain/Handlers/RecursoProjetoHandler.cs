using Cpnucleo.Domain.Commands.Requests.RecursoProjeto;
using Cpnucleo.Domain.Commands.Responses.RecursoProjeto;
using Cpnucleo.Domain.Queries.Requests.RecursoProjeto;
using Cpnucleo.Domain.Queries.Responses.RecursoProjeto;
using Cpnucleo.Domain.UoW;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class RecursoProjetoHandler :
        IRequestHandler<CreateRecursoProjetoComand, CreateRecursoProjetoResponse>,
        IRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoResponse>,
        IRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoResponse>,
        IRequestHandler<RemoveRecursoProjetoComand, RemoveRecursoProjetoResponse>,
        IRequestHandler<UpdateRecursoProjetoComand, UpdateRecursoProjetoResponse>,
        IRequestHandler<GetByProjetoQuery, GetByProjetoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecursoProjetoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateRecursoProjetoResponse> Handle(CreateRecursoProjetoComand request, CancellationToken cancellationToken)
        {
            CreateRecursoProjetoResponse result = new CreateRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoProjeto = await _unitOfWork.RecursoProjetoRepository.AddAsync(request.RecursoProjeto);

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

            result.RecursoProjeto = await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListRecursoProjetoResponse> Handle(ListRecursoProjetoQuery request, CancellationToken cancellationToken)
        {
            ListRecursoProjetoResponse result = new ListRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            result.RecursoProjetos = await _unitOfWork.RecursoProjetoRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveRecursoProjetoResponse> Handle(RemoveRecursoProjetoComand request, CancellationToken cancellationToken)
        {
            RemoveRecursoProjetoResponse result = new RemoveRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            Entities.RecursoProjeto obj = await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.RecursoProjetoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateRecursoProjetoResponse> Handle(UpdateRecursoProjetoComand request, CancellationToken cancellationToken)
        {
            UpdateRecursoProjetoResponse result = new UpdateRecursoProjetoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.RecursoProjetoRepository.Update(request.RecursoProjeto);

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

            result.RecursoProjetos = await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(request.IdProjeto);
            result.Status = OperationResult.Success;

            return result;
        }
    }
}
