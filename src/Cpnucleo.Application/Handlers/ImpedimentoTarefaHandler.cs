using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

namespace Cpnucleo.Application.Handlers;

public class ImpedimentoTarefaHandler :
    IAsyncRequestHandler<CreateImpedimentoTarefaCommand, CreateImpedimentoTarefaResponse>,
    IAsyncRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaResponse>,
    IAsyncRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaResponse>,
    IAsyncRequestHandler<RemoveImpedimentoTarefaCommand, RemoveImpedimentoTarefaResponse>,
    IAsyncRequestHandler<UpdateImpedimentoTarefaCommand, UpdateImpedimentoTarefaResponse>,
    IAsyncRequestHandler<GetByTarefaQuery, GetByTarefaResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ImpedimentoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<CreateImpedimentoTarefaResponse> InvokeAsync(CreateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        CreateImpedimentoTarefaResponse result = new CreateImpedimentoTarefaResponse
        {
            Status = OperationResult.Failed
        };

        ImpedimentoTarefa obj = await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(_mapper.Map<ImpedimentoTarefa>(request.ImpedimentoTarefa));
        result.ImpedimentoTarefa = _mapper.Map<ImpedimentoTarefaViewModel>(obj);

        await _unitOfWork.SaveChangesAsync();

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<GetImpedimentoTarefaResponse> InvokeAsync(GetImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        GetImpedimentoTarefaResponse result = new GetImpedimentoTarefaResponse
        {
            Status = OperationResult.Failed
        };

        result.ImpedimentoTarefa = _mapper.Map<ImpedimentoTarefaViewModel>(await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id));

        if (result.ImpedimentoTarefa == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<ListImpedimentoTarefaResponse> InvokeAsync(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        ListImpedimentoTarefaResponse result = new ListImpedimentoTarefaResponse
        {
            Status = OperationResult.Failed
        };

        result.ImpedimentoTarefas = _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.AllAsync(request.GetDependencies));
        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<RemoveImpedimentoTarefaResponse> InvokeAsync(RemoveImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        RemoveImpedimentoTarefaResponse result = new RemoveImpedimentoTarefaResponse
        {
            Status = OperationResult.Failed
        };

        ImpedimentoTarefa obj = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<UpdateImpedimentoTarefaResponse> InvokeAsync(UpdateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        UpdateImpedimentoTarefaResponse result = new UpdateImpedimentoTarefaResponse
        {
            Status = OperationResult.Failed
        };

        _unitOfWork.ImpedimentoTarefaRepository.Update(_mapper.Map<ImpedimentoTarefa>(request.ImpedimentoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<GetByTarefaResponse> InvokeAsync(GetByTarefaQuery request, CancellationToken cancellationToken)
    {
        GetByTarefaResponse result = new GetByTarefaResponse
        {
            Status = OperationResult.Failed
        };

        result.ImpedimentoTarefas = _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(request.IdTarefa));
        result.Status = OperationResult.Success;

        return result;
    }
}
