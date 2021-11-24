using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

namespace Cpnucleo.Application.Handlers;

public class ImpedimentoTarefaHandler :
    IAsyncRequestHandler<CreateImpedimentoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<GetImpedimentoTarefaQuery, ImpedimentoTarefaViewModel>,
    IAsyncRequestHandler<ListImpedimentoTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>>,
    IAsyncRequestHandler<RemoveImpedimentoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<GetByTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ImpedimentoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<OperationResult> InvokeAsync(CreateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(_mapper.Map<ImpedimentoTarefa>(request.ImpedimentoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<ImpedimentoTarefaViewModel> InvokeAsync(GetImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        ImpedimentoTarefaViewModel result = _mapper.Map<ImpedimentoTarefaViewModel>(await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id));

        return result;
    }

    public async ValueTask<IEnumerable<ImpedimentoTarefaViewModel>> InvokeAsync(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ImpedimentoTarefaViewModel> result = _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(RemoveImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        ImpedimentoTarefa obj = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(UpdateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.ImpedimentoTarefaRepository.Update(_mapper.Map<ImpedimentoTarefa>(request.ImpedimentoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<IEnumerable<ImpedimentoTarefaViewModel>> InvokeAsync(GetByTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ImpedimentoTarefaViewModel> result = _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(request.IdTarefa));

        return result;
    }
}
