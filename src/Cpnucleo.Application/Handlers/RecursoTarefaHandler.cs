using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

namespace Cpnucleo.Application.Handlers;

public class RecursoTarefaHandler :
    IAsyncRequestHandler<CreateRecursoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<GetRecursoTarefaQuery, RecursoTarefaViewModel>,
    IAsyncRequestHandler<ListRecursoTarefaQuery, IEnumerable<RecursoTarefaViewModel>>,
    IAsyncRequestHandler<RemoveRecursoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<UpdateRecursoTarefaCommand, OperationResult>,
    IAsyncRequestHandler<GetByTarefaQuery, IEnumerable<RecursoTarefaViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RecursoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<OperationResult> InvokeAsync(CreateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.RecursoTarefaRepository.AddAsync(_mapper.Map<RecursoTarefa>(request.RecursoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<RecursoTarefaViewModel> InvokeAsync(GetRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        RecursoTarefaViewModel result = _mapper.Map<RecursoTarefaViewModel>(await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id));

        return result;
    }

    public async ValueTask<IEnumerable<RecursoTarefaViewModel>> InvokeAsync(ListRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RecursoTarefaViewModel> result = _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(RemoveRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        RecursoTarefa obj = await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(UpdateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.RecursoTarefaRepository.Update(_mapper.Map<RecursoTarefa>(request.RecursoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<IEnumerable<RecursoTarefaViewModel>> InvokeAsync(GetByTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RecursoTarefaViewModel> result = _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(request.IdTarefa));

        return result;
    }
}
