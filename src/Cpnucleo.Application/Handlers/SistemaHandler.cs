using Cpnucleo.Infra.CrossCutting.Bus.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Events.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;

namespace Cpnucleo.Application.Handlers;

public class SistemaHandler :
    IAsyncRequestHandler<CreateSistemaCommand, CreateSistemaResponse>,
    IAsyncRequestHandler<GetSistemaQuery, GetSistemaResponse>,
    IAsyncRequestHandler<ListSistemaQuery, ListSistemaResponse>,
    IAsyncRequestHandler<RemoveSistemaCommand, RemoveSistemaResponse>,
    IAsyncRequestHandler<UpdateSistemaCommand, UpdateSistemaResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventHandler _eventHandler;

    public SistemaHandler(IUnitOfWork unitOfWork, IMapper mapper, IEventHandler eventHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _eventHandler = eventHandler;
    }

    public async ValueTask<CreateSistemaResponse> InvokeAsync(CreateSistemaCommand request, CancellationToken cancellationToken = default)
    {
        CreateSistemaResponse result = new()
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
        GetSistemaResponse result = new()
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
        ListSistemaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Sistemas = _mapper.Map<IEnumerable<SistemaViewModel>>(await _unitOfWork.SistemaRepository.AllAsync(request.GetDependencies));
        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<RemoveSistemaResponse> InvokeAsync(RemoveSistemaCommand request, CancellationToken cancellationToken = default)
    {
        RemoveSistemaResponse result = new()
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

        if (result.Status == OperationResult.Success)
        {
            await _eventHandler.PublishEventAsync(new RemoveSistemaEvent { Id = request.Id });
        }

        return result;
    }

    public async ValueTask<UpdateSistemaResponse> InvokeAsync(UpdateSistemaCommand request, CancellationToken cancellationToken = default)
    {
        UpdateSistemaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        _unitOfWork.SistemaRepository.Update(_mapper.Map<Sistema>(request.Sistema));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
