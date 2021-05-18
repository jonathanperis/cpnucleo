using Cpnucleo.Domain.Commands.Requests.Recurso;
using Cpnucleo.Domain.Commands.Responses.Recurso;
using Cpnucleo.Domain.Queries.Requests.Recurso;
using Cpnucleo.Domain.Queries.Responses.Recurso;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class RecursoHandler :
        IRequestHandler<CreateRecursoComand, CreateRecursoResponse>,
        IRequestHandler<GetRecursoQuery, GetRecursoResponse>,
        IRequestHandler<ListRecursoQuery, ListRecursoResponse>,
        IRequestHandler<RemoveRecursoComand, RemoveRecursoResponse>,
        IRequestHandler<UpdateRecursoComand, UpdateRecursoResponse>,
        IRequestHandler<AuthQuery, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyManager _cryptographyManager;

        public RecursoHandler(IUnitOfWork unitOfWork, ICryptographyManager cryptographyManager)
        {
            _unitOfWork = unitOfWork;
            _cryptographyManager = cryptographyManager;
        }

        public async Task<CreateRecursoResponse> Handle(CreateRecursoComand request, CancellationToken cancellationToken)
        {
            CreateRecursoResponse result = new CreateRecursoResponse
            {
                Status = OperationResult.Failed
            };

            result.Recurso = await _unitOfWork.RecursoRepository.AddAsync(request.Recurso);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetRecursoResponse> Handle(GetRecursoQuery request, CancellationToken cancellationToken)
        {
            GetRecursoResponse result = new GetRecursoResponse
            {
                Status = OperationResult.Failed
            };

            result.Recurso = await _unitOfWork.RecursoRepository.GetAsync(request.Id);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListRecursoResponse> Handle(ListRecursoQuery request, CancellationToken cancellationToken)
        {
            ListRecursoResponse result = new ListRecursoResponse
            {
                Status = OperationResult.Failed
            };

            result.Recursos = await _unitOfWork.RecursoRepository.AllAsync(request.GetDependencies);
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveRecursoResponse> Handle(RemoveRecursoComand request, CancellationToken cancellationToken)
        {
            RemoveRecursoResponse result = new RemoveRecursoResponse
            {
                Status = OperationResult.Failed
            };

            Entities.Recurso obj = await _unitOfWork.RecursoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.RecursoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateRecursoResponse> Handle(UpdateRecursoComand request, CancellationToken cancellationToken)
        {
            UpdateRecursoResponse result = new UpdateRecursoResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.RecursoRepository.Update(request.Recurso);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<AuthResponse> Handle(AuthQuery request, CancellationToken cancellationToken)
        {
            AuthResponse result = new AuthResponse
            {
                Status = OperationResult.Failed
            };

            result.Recurso = await _unitOfWork.RecursoRepository.GetByLoginAsync(request.Login);

            if (result.Recurso == null)
            {
                return result;
            }

            bool success = _cryptographyManager.VerifyPbkdf2(request.Senha, result.Recurso.Senha, result.Recurso.Salt);

            result.Recurso.Senha = null;
            result.Recurso.Salt = null;

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
