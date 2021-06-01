using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class RecursoHandler :
        IRequestHandler<CreateRecursoCommand, CreateRecursoResponse>,
        IRequestHandler<GetRecursoQuery, GetRecursoResponse>,
        IRequestHandler<ListRecursoQuery, ListRecursoResponse>,
        IRequestHandler<RemoveRecursoCommand, RemoveRecursoResponse>,
        IRequestHandler<UpdateRecursoCommand, UpdateRecursoResponse>,
        IRequestHandler<AuthQuery, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICryptographyManager _cryptographyManager;

        public RecursoHandler(IUnitOfWork unitOfWork, IMapper mapper, ICryptographyManager cryptographyManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cryptographyManager = cryptographyManager;
        }

        public async Task<CreateRecursoResponse> Handle(CreateRecursoCommand request, CancellationToken cancellationToken)
        {
            CreateRecursoResponse result = new CreateRecursoResponse
            {
                Status = OperationResult.Failed
            };

            _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

            request.Recurso.Senha = senhaCrypt;
            request.Recurso.Salt = salt;

            Recurso obj = await _unitOfWork.RecursoRepository.AddAsync(_mapper.Map<Recurso>(request.Recurso));
            result.Recurso = _mapper.Map<RecursoViewModel>(obj);

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

            result.Recurso = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetAsync(request.Id));

            result.Recurso.Senha = null;
            result.Recurso.Salt = null;

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListRecursoResponse> Handle(ListRecursoQuery request, CancellationToken cancellationToken)
        {
            ListRecursoResponse result = new ListRecursoResponse
            {
                Status = OperationResult.Failed
            };

            result.Recursos = _mapper.Map<IEnumerable<RecursoViewModel>>(await _unitOfWork.RecursoRepository.AllAsync(request.GetDependencies));

            foreach (RecursoViewModel item in result.Recursos)
            {
                item.Senha = null;
                item.Salt = null;
            }

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveRecursoResponse> Handle(RemoveRecursoCommand request, CancellationToken cancellationToken)
        {
            RemoveRecursoResponse result = new RemoveRecursoResponse
            {
                Status = OperationResult.Failed
            };

            Recurso obj = await _unitOfWork.RecursoRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.RecursoRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateRecursoResponse> Handle(UpdateRecursoCommand request, CancellationToken cancellationToken)
        {
            UpdateRecursoResponse result = new UpdateRecursoResponse
            {
                Status = OperationResult.Failed
            };

            _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

            request.Recurso.Senha = senhaCrypt;
            request.Recurso.Salt = salt;

            _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(request.Recurso));

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

            result.Recurso = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetByLoginAsync(request.Login));

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
