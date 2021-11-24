using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

namespace Cpnucleo.Application.Handlers;

public class WorkflowHandler :
    IAsyncRequestHandler<CreateWorkflowCommand, OperationResult>,
    IAsyncRequestHandler<GetWorkflowQuery, WorkflowViewModel>,
    IAsyncRequestHandler<ListWorkflowQuery, IEnumerable<WorkflowViewModel>>,
    IAsyncRequestHandler<RemoveWorkflowCommand, OperationResult>,
    IAsyncRequestHandler<UpdateWorkflowCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<OperationResult> InvokeAsync(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.WorkflowRepository.AddAsync(_mapper.Map<Workflow>(request.Workflow));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<WorkflowViewModel> InvokeAsync(GetWorkflowQuery request, CancellationToken cancellationToken)
    {
        WorkflowViewModel result = _mapper.Map<WorkflowViewModel>(await _unitOfWork.WorkflowRepository.GetAsync(request.Id));

        return result;
    }

    public async ValueTask<IEnumerable<WorkflowViewModel>> InvokeAsync(ListWorkflowQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<WorkflowViewModel> result = _mapper.Map<IEnumerable<WorkflowViewModel>>(await _unitOfWork.WorkflowRepository.AllAsync(request.GetDependencies));

        await PreencherDadosAdicionaisAsync(result);

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(RemoveWorkflowCommand request, CancellationToken cancellationToken)
    {
        Workflow obj = await _unitOfWork.WorkflowRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.WorkflowRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(UpdateWorkflowCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.WorkflowRepository.Update(_mapper.Map<Workflow>(request.Workflow));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    private async Task PreencherDadosAdicionaisAsync(IEnumerable<WorkflowViewModel> lista)
    {
        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        foreach (WorkflowViewModel item in lista)
        {
            item.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
        }
    }
}
