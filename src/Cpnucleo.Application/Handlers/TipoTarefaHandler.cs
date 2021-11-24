using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

namespace Cpnucleo.Application.Handlers;

public class TipoTarefaHandler :
    IAsyncRequestHandler<CreateTipoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<GetTipoTarefaQuery, TipoTarefaViewModel>,
    IAsyncRequestHandler<ListTipoTarefaQuery, IEnumerable<TipoTarefaViewModel>>,
    IAsyncRequestHandler<RemoveTipoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<UpdateTipoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TipoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<OperationResult> InvokeAsync(CreateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.TipoTarefaRepository.AddAsync(_mapper.Map<TipoTarefa>(request.TipoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<TipoTarefaViewModel> InvokeAsync(GetTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        TipoTarefaViewModel result = _mapper.Map<TipoTarefaViewModel>(await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id));

        return result;
    }

    public async ValueTask<IEnumerable<TipoTarefaViewModel>> InvokeAsync(ListTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TipoTarefaViewModel> result = _mapper.Map<IEnumerable<TipoTarefaViewModel>>(await _unitOfWork.TipoTarefaRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(RemoveTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        TipoTarefa obj = await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.TipoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(UpdateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.TipoTarefaRepository.Update(_mapper.Map<TipoTarefa>(request.TipoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
