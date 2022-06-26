namespace Cpnucleo.Application.Queries.Workflow;

public class ListWorkflowHandler : IRequestHandler<ListWorkflowQuery, ListWorkflowViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWorkflowService _workflowService;

    public ListWorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper, IWorkflowService workflowService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _workflowService = workflowService;
    }

    public async Task<ListWorkflowViewModel> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Workflow> workflows = await _unitOfWork.WorkflowRepository.AllAsync(request.GetDependencies);

        if (workflows == null)
        {
            return new ListWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<WorkflowDTO> result = _mapper.Map<IEnumerable<WorkflowDTO>>(workflows);

        await PreencherDadosAdicionaisAsync(result);

        return new ListWorkflowViewModel { Workflows = result, OperationResult = OperationResult.Success };
    }

    private async Task PreencherDadosAdicionaisAsync(IEnumerable<WorkflowDTO> lista)
    {
        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        foreach (WorkflowDTO item in lista)
        {
            item.TamanhoColuna = _workflowService.GetTamanhoColuna(colunas);
        }
    }
}
