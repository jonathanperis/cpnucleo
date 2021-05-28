using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class SistemaHandler :
        IRequestHandler<CreateSistemaCommand, CreateSistemaResponse>,
        IRequestHandler<GetSistemaQuery, GetSistemaResponse>,
        IRequestHandler<ListSistemaQuery, ListSistemaResponse>,
        IRequestHandler<RemoveSistemaCommand, RemoveSistemaResponse>,
        IRequestHandler<UpdateSistemaCommand, UpdateSistemaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SistemaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateSistemaResponse> Handle(CreateSistemaCommand request, CancellationToken cancellationToken)
        {
            CreateSistemaResponse result = new CreateSistemaResponse
            {
                Status = OperationResult.Failed
            };

            Sistema obj = await _unitOfWork.SistemaRepository.AddAsync(_mapper.Map<Sistema>(request.Sistema));
            result.Sistema = _mapper.Map<SistemaViewModel>(obj);

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

            result.Sistema = _mapper.Map<SistemaViewModel>(await _unitOfWork.SistemaRepository.GetAsync(request.Id));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListSistemaResponse> Handle(ListSistemaQuery request, CancellationToken cancellationToken)
        {
            ListSistemaResponse result = new ListSistemaResponse
            {
                Status = OperationResult.Failed
            };

            result.Sistemas = _mapper.Map<IEnumerable<SistemaViewModel>>(await _unitOfWork.SistemaRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<RemoveSistemaResponse> Handle(RemoveSistemaCommand request, CancellationToken cancellationToken)
        {
            RemoveSistemaResponse result = new RemoveSistemaResponse
            {
                Status = OperationResult.Failed
            };

            Sistema obj = await _unitOfWork.SistemaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.SistemaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateSistemaResponse> Handle(UpdateSistemaCommand request, CancellationToken cancellationToken)
        {
            UpdateSistemaResponse result = new UpdateSistemaResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.SistemaRepository.Update(_mapper.Map<Sistema>(request.Sistema));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }
    }
}
