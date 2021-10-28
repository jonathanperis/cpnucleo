using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;

namespace Cpnucleo.Application.Handlers;

public class RecursoTarefaHandler :
    IAsyncRequestHandler<CreateRecursoTarefaCommand, CreateRecursoTarefaResponse>,
    IAsyncRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaResponse>,
    IAsyncRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaResponse>,
    IAsyncRequestHandler<RemoveRecursoTarefaCommand, RemoveRecursoTarefaResponse>,
    IAsyncRequestHandler<UpdateRecursoTarefaCommand, UpdateRecursoTarefaResponse>,
    IAsyncRequestHandler<GetByTarefaQuery, GetByTarefaResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RecursoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<CreateRecursoTarefaResponse> InvokeAsync(CreateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        CreateRecursoTarefaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        RecursoTarefa obj = await _unitOfWork.RecursoTarefaRepository.AddAsync(_mapper.Map<RecursoTarefa>(request.RecursoTarefa));
        result.RecursoTarefa = _mapper.Map<RecursoTarefaViewModel>(obj);

        await _unitOfWork.SaveChangesAsync();

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<GetRecursoTarefaResponse> InvokeAsync(GetRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        GetRecursoTarefaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.RecursoTarefa = _mapper.Map<RecursoTarefaViewModel>(await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id));

        if (result.RecursoTarefa == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<ListRecursoTarefaResponse> InvokeAsync(ListRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        ListRecursoTarefaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.RecursoTarefas = _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.AllAsync(request.GetDependencies));
        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<RemoveRecursoTarefaResponse> InvokeAsync(RemoveRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        RemoveRecursoTarefaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        RecursoTarefa obj = await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        await _unitOfWork.RecursoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<UpdateRecursoTarefaResponse> InvokeAsync(UpdateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        UpdateRecursoTarefaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        _unitOfWork.RecursoTarefaRepository.Update(_mapper.Map<RecursoTarefa>(request.RecursoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<GetByTarefaResponse> InvokeAsync(GetByTarefaQuery request, CancellationToken cancellationToken)
    {
        GetByTarefaResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.RecursoTarefas = _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(request.IdTarefa));
        result.Status = OperationResult.Success;

        return result;
    }
}
