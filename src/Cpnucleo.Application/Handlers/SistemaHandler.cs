using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MessagePipe;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Handlers
{
    public class SistemaHandler :
        IAsyncRequestHandler<CreateSistemaCommand, CreateSistemaResponse>,
        IAsyncRequestHandler<GetSistemaQuery, GetSistemaResponse>,
        IAsyncRequestHandler<ListSistemaQuery, ListSistemaResponse>,
        IAsyncRequestHandler<RemoveSistemaCommand, RemoveSistemaResponse>,
        IAsyncRequestHandler<UpdateSistemaCommand, UpdateSistemaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SistemaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<CreateSistemaResponse> InvokeAsync(CreateSistemaCommand request, CancellationToken cancellationToken = default)
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

        public async ValueTask<GetSistemaResponse> InvokeAsync(GetSistemaQuery request, CancellationToken cancellationToken = default)
        {
            GetSistemaResponse result = new GetSistemaResponse
            {
                Status = OperationResult.Failed
            };

            result.Sistema = _mapper.Map<SistemaViewModel>(await _unitOfWork.SistemaRepository.GetAsync(request.Id));

            if (result.Sistema == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<ListSistemaResponse> InvokeAsync(ListSistemaQuery request, CancellationToken cancellationToken = default)
        {
            ListSistemaResponse result = new ListSistemaResponse
            {
                Status = OperationResult.Failed
            };

            result.Sistemas = _mapper.Map<IEnumerable<SistemaViewModel>>(await _unitOfWork.SistemaRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            return result;
        }

        public async ValueTask<RemoveSistemaResponse> InvokeAsync(RemoveSistemaCommand request, CancellationToken cancellationToken = default)
        {
            RemoveSistemaResponse result = new RemoveSistemaResponse
            {
                Status = OperationResult.Failed
            };

            Sistema obj = await _unitOfWork.SistemaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                result.Status = OperationResult.NotFound;

                return result;
            }

            await _unitOfWork.SistemaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async ValueTask<UpdateSistemaResponse> InvokeAsync(UpdateSistemaCommand request, CancellationToken cancellationToken = default)
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
