namespace Cpnucleo.Application.Queries.Workflow;

public sealed class ListWorkflowHandler : IRequestHandler<ListWorkflowQuery, ListWorkflowViewModel>
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
        List<WorkflowDTO> workflows = await _unitOfWork.WorkflowRepository.All(request.GetDependencies)
            .ProjectTo<WorkflowDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (workflows is null)
        {
            return new ListWorkflowViewModel { OperationResult = OperationResult.NotFound };
        }

        await PreencherDadosAdicionaisAsync(workflows);

        return new ListWorkflowViewModel { Workflows = workflows, OperationResult = OperationResult.Success };
    }

    private async Task PreencherDadosAdicionaisAsync(List<WorkflowDTO> lista)
    {
        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        foreach (WorkflowDTO item in lista)
        {
            item.TamanhoColuna = _workflowService.GetTamanhoColuna(colunas);
        }
    }
}
